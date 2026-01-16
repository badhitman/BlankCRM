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
    public async Task<DocumentNewVersionResponseModel> CreateRowRetailDocumentAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB> req, CancellationToken token = default)
    {
        string msg;
        DocumentNewVersionResponseModel res = new();

        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (req.Payload.Quantity <= 0)
        {
            res.AddError("Количество должно быть больше нуля");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DocumentRetailModelDB retailOrderDb = await context.OrdersRetail
            .FirstAsync(x => x.Id == req.Payload.OrderId, cancellationToken: token);

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        LockTransactionModelDB locker = new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.Payload.OfferId,
            LockerAreaId = retailOrderDb.WarehouseId,
            Marker = nameof(CreateRowRetailDocumentAsync),
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
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return res;
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => x.OfferId == req.Payload.OfferId)
           .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv = offerAvailabilityDB
            .Where(x => x.OfferId == req.Payload.OfferId && x.WarehouseId == retailOrderDb.WarehouseId)
            .FirstOrDefault();

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
            .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        if (!offOrdersStatuses.Contains(retailOrderDb.StatusDocument))
        {
            if (res_WarehouseReserveForRetailOrder.Response != true)
            {
                if (regOfferAv is null)
                {
                    msg = $"Количество [offer: #{req.Payload.OfferId} '{req.Payload.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError($"{msg}. Баланс не может быть отрицательным");
                    return res;
                }
                else if (regOfferAv.Quantity < req.Payload.Quantity)
                {
                    msg = $"Количество [offer: #{req.Payload.OfferId} '{req.Payload.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError($"{msg}. Баланс не может быть отрицательным");
                    return res;
                }

                await context.OffersAvailability
                       .Where(x => x.Id == regOfferAv.Id)
                           .ExecuteUpdateAsync(set => set
                               .SetProperty(p => p.Quantity, p => p.Quantity - req.Payload.Quantity), cancellationToken: token);
            }
            else
            {
                if (regOfferAv is null)
                {
                    await context.OffersAvailability.AddAsync(new()
                    {
                        OfferId = req.Payload.OfferId,
                        NomenclatureId = req.Payload.NomenclatureId,
                        WarehouseId = retailOrderDb.WarehouseId,
                        Quantity = req.Payload.Quantity,
                    }, token);
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                            .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + req.Payload.Quantity), cancellationToken: token);
                }
            }
        }

        req.Payload.Version = Guid.NewGuid();
        req.Payload.Order = null;
        req.Payload.Nomenclature = null;
        req.Payload.Offer = null;

        await context.RowsOrdersRetails.AddAsync(req.Payload, token);
        await context.SaveChangesAsync(token);
        res.Response = req.Payload.Id;
        await context.LockTransactions
            .Where(x => x.Id == locker.Id)
            .ExecuteDeleteAsync(cancellationToken: token);

        res.DocumentNewVersion = Guid.NewGuid();
        await context.OrdersRetail
          .Where(x => x.Id == req.Payload.OrderId)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Version, res.DocumentNewVersion), cancellationToken: token);

        await transaction.CommitAsync(token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateRowRetailDocumentAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB> req, CancellationToken token = default)
    {
        TResponseModel<Guid?> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        if (req.Payload.Quantity <= 0)
            return new()
            {
                Messages = [new() {
                TypeMessage = MessagesTypesEnum.Error,
                Text = "Количество должно быть больше нуля" }]
            };

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
            .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        DocumentRetailModelDB retailOrderDb = await context.OrdersRetail
            .FirstAsync(x => x.Id == req.Payload.OrderId, cancellationToken: token);
        loggerRepo.LogInformation($"{nameof(retailOrderDb)}: {JsonConvert.SerializeObject(retailOrderDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        RowOfRetailOrderDocumentModelDB? rowDb = await context.RowsOrdersRetails
            .Include(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature)
            .FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);

        if (rowDb.Version != req.Payload.Version)
            return new()
            {
                Messages = [new() {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = $"Строку документа уже кто-то изменил. Обновите документ и попробуйте изменить его снова" }]
            };

        if (offOrdersStatuses.Contains(retailOrderDb.StatusDocument))
        {
            await context.RowsOrdersRetails
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Comment, req.Payload.Comment)
                    .SetProperty(p => p.Quantity, req.Payload.Quantity)
                    .SetProperty(p => p.WeightOffer, req.Payload.WeightOffer)
                    .SetProperty(p => p.Version, Guid.NewGuid())
                    .SetProperty(p => p.Amount, req.Payload.Amount), cancellationToken: token);

            retailOrderDb.Version = Guid.NewGuid();
            await context.OrdersRetail
              .Where(x => x.Id == req.Payload.OrderId)
              .ExecuteUpdateAsync(set => set
                  .SetProperty(p => p.Version, retailOrderDb.Version), cancellationToken: token);

            return new()
            {
                Response = retailOrderDb.Version,
                Messages = [new() {
                    Text = "Ok",
                    TypeMessage = MessagesTypesEnum.Success }]
            };
        }

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = req.Payload.OfferId,
                LockerAreaId = retailOrderDb.WarehouseId,
                Marker = nameof(UpdateRowRetailDocumentAsync),
            }];
        if (rowDb.OfferId != req.Payload.OfferId)
            lockers.Add(new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowDb.OfferId,
                LockerAreaId = retailOrderDb.WarehouseId,
                Marker = nameof(UpdateRowRetailDocumentAsync),
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
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}{ex.Message}" }] };
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
            .OffersAvailability
            .Where(x => x.OfferId == req.Payload.OfferId || x.OfferId == rowDb.OfferId)
            .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv;
        if (rowDb.OfferId != req.Payload.OfferId)
        {
            regOfferAv = offerAvailabilityDB
                .FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == retailOrderDb.WarehouseId);

            if (res_WarehouseReserveForRetailOrder.Response == true)
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{rowDb.OfferId} '{rowDb.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(rowDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}. Баланс не может быть отрицательным" }] };
                }
                else if (regOfferAv.Quantity < rowDb.Quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{rowDb.OfferId} '{rowDb.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(rowDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}. Баланс не может быть отрицательным" }] };
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
                        WarehouseId = retailOrderDb.WarehouseId,
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
            .Where(x => x.OfferId == req.Payload.OfferId && x.WarehouseId == retailOrderDb.WarehouseId)
            .FirstOrDefault();

        decimal _quantity = rowDb.OfferId != req.Payload.OfferId
            ? req.Payload.Quantity
            : req.Payload.Quantity - rowDb.Quantity;

        if (res_WarehouseReserveForRetailOrder.Response == true)
        {
            if (_quantity < 0)
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{req.Payload.OfferId} '{req.Payload.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}. Баланс не может быть отрицательным" }] };
                }
                else if (regOfferAv.Quantity + _quantity < 0)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{req.Payload.OfferId} '{req.Payload.Offer?.GetName()}'] не может быть списано (остаток {regOfferAv.Quantity})";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}. Баланс не может быть отрицательным" }] };
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
                        OfferId = req.Payload.OfferId,
                        NomenclatureId = req.Payload.NomenclatureId,
                        WarehouseId = retailOrderDb.WarehouseId,
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
                        WarehouseId = retailOrderDb.WarehouseId,
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
                    msg = $"На складе #{retailOrderDb.WarehouseId} отсутствует офер #{req.Payload.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = msg }] };
                }
                else if (regOfferAv.Quantity < _quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{retailOrderDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = msg }] };
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
          .Where(x => x.Id == req.Payload.Id)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Comment, req.Payload.Comment)
              .SetProperty(p => p.Quantity, req.Payload.Quantity)
              .SetProperty(p => p.WeightOffer, req.Payload.WeightOffer)
              .SetProperty(p => p.Version, Guid.NewGuid())
              .SetProperty(p => p.Amount, req.Payload.Amount), cancellationToken: token);

        retailOrderDb.Version = Guid.NewGuid();
        await context.OrdersRetail
          .Where(x => x.Id == req.Payload.OrderId)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Version, retailOrderDb.Version), cancellationToken: token);

        await transaction.CommitAsync(token);
        return new()
        {
            Response = retailOrderDb.Version,
            Messages = [new() {
                TypeMessage = MessagesTypesEnum.Info,
                Text = "Ok" }]
        };
    }

    /// <inheritdoc/>
    public async Task<DeleteRowRetailDocumentResponseModel> DeleteRowRetailDocumentAsync(TAuthRequestStandardModel<DeleteRowRetailDocumentRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "req.Payload is null",
                }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        RowOfRetailOrderDocumentModelDB rowOfRetailDb = await context.RowsOrdersRetails
            .Include(x => x.Order)
            .FirstAsync(x => x.Id == req.Payload.RowId, cancellationToken: token);

        DocumentRetailModelDB retailOrderDb = rowOfRetailDb.Order!;
        DocumentRetailModelDB _order = rowOfRetailDb.Order!;
        rowOfRetailDb.Order = null;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        if (!offOrdersStatuses.Contains(retailOrderDb.StatusDocument))
        {
            TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
                .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

            List<LockTransactionModelDB> lockers = [new()
            {
                LockerName = nameof(OfferAvailabilityModelDB),
                LockerId = rowOfRetailDb.OfferId,
                LockerAreaId = retailOrderDb.WarehouseId,
                Marker = nameof(DeleteRowRetailDocumentAsync),
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
                loggerRepo.LogError(ex, $"{msg} #{req.Payload.RowId}");
                return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}{ex.Message}" }] };
            }

            OfferAvailabilityModelDB? regOfferAv = await context
                .OffersAvailability
                .Where(x => x.OfferId == rowOfRetailDb.OfferId)
                .FirstOrDefaultAsync(x => x.OfferId == rowOfRetailDb.OfferId && x.WarehouseId == retailOrderDb.WarehouseId, cancellationToken: token);

            if (res_WarehouseReserveForRetailOrder.Response == true)
            {
                if (regOfferAv is null)
                {
                    if (req.Payload.ForceDelete)
                    {
                        regOfferAv = new()
                        {
                            WarehouseId = retailOrderDb.WarehouseId,
                            NomenclatureId = rowOfRetailDb.NomenclatureId,
                            OfferId = rowOfRetailDb.OfferId,
                            Quantity = -rowOfRetailDb.Quantity,
                        };
                        await context.OffersAvailability.AddAsync(regOfferAv, token);
                        await context.SaveChangesAsync(token);
                    }
                    else
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{retailOrderDb.WarehouseId} отсутствует офер #{rowOfRetailDb.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(rowOfRetailDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = msg }] };
                    }
                }
                else if (regOfferAv.Quantity < rowOfRetailDb.Quantity && !req.Payload.ForceDelete)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{retailOrderDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(rowOfRetailDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = msg }] };
                }
                else
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - rowOfRetailDb.Quantity), cancellationToken: token);
            }
            else
            {
                if (regOfferAv is null)
                {
                    regOfferAv = new()
                    {
                        WarehouseId = retailOrderDb.WarehouseId,
                        Quantity = rowOfRetailDb.Quantity,
                        OfferId = rowOfRetailDb.OfferId,
                        NomenclatureId = rowOfRetailDb.NomenclatureId,
                    };
                    await context.OffersAvailability.AddAsync(regOfferAv, token);
                }
                else
                {
                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity + rowOfRetailDb.Quantity), cancellationToken: token);
                }
            }

            if (lockers.Count != 0)
                context.RemoveRange(lockers);

            await context.SaveChangesAsync(token);
        }

        await context.RowsOrdersRetails
            .Where(x => x.Id == req.Payload.RowId)
            .ExecuteDeleteAsync(cancellationToken: token);

        Guid _nv = Guid.NewGuid();
        await context.OrdersRetail
            .Where(x => x.Id == retailOrderDb.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Version, _nv), cancellationToken: token);

        await transaction.CommitAsync(token);

        return new()
        {
            Response = rowOfRetailDb,
            DocumentNewVersion = _nv,
            Messages = [new() { TypeMessage = MessagesTypesEnum.Info, Text = "Строка документа удалена" }]
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default)
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