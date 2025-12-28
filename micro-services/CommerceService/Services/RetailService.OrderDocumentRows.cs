////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default)
    {
        string msg;
        TResponseModel<int> res = new();

        if (req.Quantity <= 0)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Количество должно быть больше нуля" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DocumentRetailModelDB docDb = await context.OrdersRetail
            .FirstAsync(x => x.Id == req.OrderId, cancellationToken: token);

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        LockTransactionModelDB locker = new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            LockerAreaId = docDb.WarehouseId,
        };

        try
        {
            await context.LockTransactions.AddAsync(locker, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            res.AddError($"{msg}{ex.Message}");
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => x.OfferId == req.OfferId)
           .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv = offerAvailabilityDB
            .Where(x => x.OfferId == req.OfferId && x.WarehouseId == docDb.WarehouseId)
            .FirstOrDefault();

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
            .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        if (!offOrdersStatuses.Contains(docDb.StatusDocument))
        {
            if (res_WarehouseReserveForRetailOrder.Response != true)
            {
                if (regOfferAv is null)
                {
                    msg = $"Количество [offer: #{req.OfferId} '{req.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError($"{msg}. Баланс не может быть отрицательным");
                    return res;
                }
                else if (regOfferAv.Quantity < req.Quantity)
                {
                    msg = $"Количество [offer: #{req.OfferId} '{req.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError($"{msg}. Баланс не может быть отрицательным");
                    return res;
                }

                await context.OffersAvailability
                       .Where(x => x.Id == regOfferAv.Id)
                           .ExecuteUpdateAsync(set => set
                               .SetProperty(p => p.Quantity, p => p.Quantity - req.Quantity), cancellationToken: token);
            }
            else
            {
                if (regOfferAv is null)
                {
                    await context.OffersAvailability.AddAsync(new()
                    {
                        OfferId = req.OfferId,
                        NomenclatureId = req.NomenclatureId,
                        WarehouseId = docDb.WarehouseId,
                        Quantity = req.Quantity,
                    }, token);
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                            .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + req.Quantity), cancellationToken: token);
                }
            }
        }

        req.Version = Guid.NewGuid();
        req.Order = null;
        req.Nomenclature = null;
        req.Offer = null;

        await context.RowsOrdersRetails.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.OrdersRetail
          .Where(x => x.Id == req.OrderId)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await context.LockTransactions
            .Where(x => x.Id == locker.Id)
            .ExecuteDeleteAsync(cancellationToken: token);

        await transaction.CommitAsync(token);
        return new TResponseModel<int>() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default)
    {
        if (req.Quantity <= 0)
            return ResponseBaseModel.CreateError("Количество должно быть больше нуля");

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
            .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DocumentRetailModelDB docDb = await context.OrdersRetail
            .FirstAsync(x => x.Id == req.OrderId, cancellationToken: token);

        RowOfOrderDocumentModelDB rowDb = await context.RowsOrders
            .Include(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .FirstAsync(x => x.Id == req.Id, cancellationToken: token);

        if (rowDb.Version != req.Version)
            return ResponseBaseModel.CreateError($"Строку документа уже кто-то изменил. Обновите документ и попробуйте изменить его снова");

        if (offOrdersStatuses.Contains(docDb.StatusDocument))
        {
            await context.RowsOrdersRetails
                .Where(x => x.Id == req.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Comment, req.Comment)
                    .SetProperty(p => p.Quantity, req.Quantity)
                    .SetProperty(p => p.Version, Guid.NewGuid())
                    .SetProperty(p => p.Amount, req.Amount), cancellationToken: token);

            await context.OrdersRetail
              .Where(x => x.Id == req.OrderId)
              .ExecuteUpdateAsync(set => set
                  .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

            return ResponseBaseModel.CreateSuccess("Ok");
        }

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.OfferId,
                LockerAreaId = docDb.WarehouseId,
            }];
        if (rowDb.OfferId != req.OfferId)
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = docDb.WarehouseId,
            });

        string msg;
        try
        {
            await context.AddRangeAsync(lockers, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            msg = $"Не удалось выполнить команду: ";
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return ResponseBaseModel.CreateError($"{msg}{ex.Message}"); ;
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
            .OffersAvailability
            .Where(x => x.OfferId == req.OfferId || x.OfferId == rowDb.OfferId)
            .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv;
        if (rowDb.OfferId != req.OfferId)
        {
            regOfferAv = offerAvailabilityDB
                .FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == docDb.WarehouseId);

            if (res_WarehouseReserveForRetailOrder.Response == true)
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{rowDb.OfferId} '{rowDb.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(rowDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return ResponseBaseModel.CreateError($"{msg}. Баланс не может быть отрицательным");
                }
                else if (regOfferAv.Quantity < rowDb.Quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{rowDb.OfferId} '{rowDb.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(rowDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return ResponseBaseModel.CreateError($"{msg}. Баланс не может быть отрицательным");
                }

                await context.OffersAvailability
                   .Where(x => x.Id == regOfferAv.Id)
                       .ExecuteUpdateAsync(set => set
                           .SetProperty(p => p.Quantity, p => p.Quantity - rowDb.Quantity), cancellationToken: token);
            }
            else
            {
                if (regOfferAv is null)
                {
                    await context.OffersAvailability.AddAsync(new()
                    {
                        OfferId = rowDb.OfferId,
                        NomenclatureId = rowDb.NomenclatureId,
                        WarehouseId = docDb.WarehouseId,
                        Quantity = rowDb.Quantity,
                    }, token);
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                           .SetProperty(p => p.Quantity, p => p.Quantity + rowDb.Quantity), cancellationToken: token);
                }
            }
        }

        regOfferAv = offerAvailabilityDB
            .Where(x => x.OfferId == req.OfferId && x.WarehouseId == docDb.WarehouseId)
            .FirstOrDefault();

        decimal _quantity = rowDb.OfferId != req.OfferId
            ? req.Quantity
            : req.Quantity - rowDb.Quantity;

        if (res_WarehouseReserveForRetailOrder.Response == true)
        {
            if (_quantity < 0)
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{req.OfferId} '{req.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return ResponseBaseModel.CreateError($"{msg}. Баланс не может быть отрицательным");
                }
                else if (regOfferAv.Quantity + _quantity < 0)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{req.OfferId} '{req.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return ResponseBaseModel.CreateError($"{msg}. Баланс не может быть отрицательным");
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                            .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + _quantity), cancellationToken: token);
                }
            }
            else
            {
                if (regOfferAv is null)
                {
                    await context.OffersAvailability.AddAsync(new()
                    {
                        OfferId = req.OfferId,
                        NomenclatureId = req.NomenclatureId,
                        WarehouseId = docDb.WarehouseId,
                        Quantity = _quantity,
                    }, token);
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                            .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + _quantity), cancellationToken: token);
                }
            }
        }
        else
        {
            if (_quantity < 0)
            {
                if (regOfferAv is not null)
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - _quantity), cancellationToken: token);
                }
                else
                {
                    regOfferAv = new()
                    {
                        NomenclatureId = rowDb.NomenclatureId,
                        WarehouseId = docDb.WarehouseId,
                        OfferId = rowDb.OfferId,
                        Quantity = -_quantity,
                    };
                    await context.OffersAvailability.AddAsync(regOfferAv, token);
                }
            }
            else
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{req.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return ResponseBaseModel.CreateError(msg);
                }
                else if (regOfferAv.Quantity < _quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return ResponseBaseModel.CreateError(msg);
                }

                await context.OffersAvailability
                    .Where(x => x.Id == regOfferAv.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity - _quantity), cancellationToken: token);
            }
        }

        if (lockers.Count != 0)
            context.RemoveRange(lockers);

        await context.SaveChangesAsync(token);

        await context.RowsOrdersRetails
          .Where(x => x.Id == req.Id)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Comment, req.Comment)
              .SetProperty(p => p.Quantity, req.Quantity)
              .SetProperty(p => p.Version, Guid.NewGuid())
              .SetProperty(p => p.Amount, req.Amount), cancellationToken: token);

        await context.OrdersRetail
          .Where(x => x.Id == req.OrderId)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteRowRetailDocumentAsync(int rowId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        RowOfRetailOrderDocumentModelDB rowDb = await context.RowsOrdersRetails
            .Include(x => x.Order)
            .FirstAsync(x => x.Id == rowId, cancellationToken: token);

        DocumentRetailModelDB docDb = rowDb.Order!;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        if (!offOrdersStatuses.Contains(docDb.StatusDocument))
        {
            TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
                .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

            List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = docDb.WarehouseId,
            }];

            string msg;
            try
            {
                await context.AddRangeAsync(lockers, token);
                await context.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось выполнить команду: ";
                loggerRepo.LogError(ex, $"{msg} #{rowId}");
                return ResponseBaseModel.CreateError($"{msg}{ex.Message}"); ;
            }

            OfferAvailabilityModelDB? regOfferAv = await context
                .OffersAvailability
                .Where(x => x.OfferId == rowDb.OfferId)
                .FirstOrDefaultAsync(x => x.OfferId == rowDb.OfferId && x.WarehouseId == docDb.WarehouseId, cancellationToken: token);

            if (res_WarehouseReserveForRetailOrder.Response == true)
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{rowDb.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(rowDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return ResponseBaseModel.CreateError(msg);
                }
                else if (regOfferAv.Quantity < rowDb.Quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(rowDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return ResponseBaseModel.CreateError(msg);
                }

                await context.OffersAvailability
                    .Where(x => x.Id == regOfferAv.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity - rowDb.Quantity), cancellationToken: token);
            }
            else
            {
                if (regOfferAv is null)
                {
                    regOfferAv = new()
                    {
                        WarehouseId = docDb.WarehouseId,
                        Quantity = rowDb.Quantity,
                        OfferId = rowDb.OfferId,
                        NomenclatureId = rowDb.NomenclatureId,
                    };
                    await context.OffersAvailability.AddAsync(regOfferAv, token);
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity + rowDb.Quantity), cancellationToken: token);
                }
            }

            if (lockers.Count != 0)
                context.RemoveRange(lockers);

            await context.SaveChangesAsync(token);
        }

        await context.RowsOrdersRetails
            .Where(x => x.Id == rowId)
            .ExecuteDeleteAsync(cancellationToken: token);

        await context.OrdersRetail
            .Where(x => x.Id == docDb.Id)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Строка документа удалена");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Status = new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }] } };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfRetailOrderDocumentModelDB> q = context.RowsOrdersRetails
            .Where(x => x.OrderId == req.Payload.OrderId)
            .AsQueryable();

        IQueryable<RowOfRetailOrderDocumentModelDB>? pq = q
            .OrderBy(x => x.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<RowOfRetailOrderDocumentModelDB> res = await pq.Include(x => x.Offer).ToListAsync(cancellationToken: token);
        foreach (RowOfRetailOrderDocumentModelDB row in res.Where(x => x.Amount <= 0 || x.WeightOffer <= 0))
        {
            if (row.Amount <= 0)
                row.Amount = row.Quantity * row.Offer!.Price;

            if (row.WeightOffer <= 0)
                row.WeightOffer = row.Quantity * row.Offer!.Weight;

            context.Update(row);
            await context.SaveChangesAsync(token);
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = res
        };
    }
}