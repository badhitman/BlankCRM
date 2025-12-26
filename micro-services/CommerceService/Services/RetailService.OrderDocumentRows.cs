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
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Количество должно быть больше нуля" }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DocumentRetailModelDB docDb = await context.OrdersRetail
            .FirstAsync(x => x.Id == req.OrderId, cancellationToken: token);

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        List<LockTransactionModelDB> lockers = [new()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = req.OfferId,
            LockerAreaId = docDb.WarehouseId,
        }];

        try
        {
            await context.AddRangeAsync(lockers, token);
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
            .FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == docDb.WarehouseId);

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
            .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        if (!ignoreStatuses.Contains(docDb.StatusDocument))
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

        if (lockers.Count != 0)
        {
            context.RemoveRange(lockers);
            await context.SaveChangesAsync(token);
        }
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
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!ignoreStatuses.Contains(docDb.StatusDocument))
        {
            RowOfOrderDocumentModelDB rowDb = await context.RowsOrders.FirstAsync(x => x.Id == req.Id, cancellationToken: token);

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

            OfferAvailabilityModelDB?
                regOfferAv = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == req.OfferId && x.WarehouseId == docDb.WarehouseId),
                regOfferAvWritingOff = offerAvailabilityDB.FirstOrDefault(x => x.OfferId == rowDb.OfferId && x.WarehouseId == docDb.WarehouseId);

            if (rowDb.OfferId != req.OfferId)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }

            if (res_WarehouseReserveForRetailOrder.Response == true)
            {

            }
            else
            {
                if (regOfferAv is null)
                {
                    if (req.Quantity < 0)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{req.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                        return ResponseBaseModel.CreateError(msg);
                    }

                    if (req.Quantity > 0)
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
                    if (regOfferAv.Quantity < -req.Quantity)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{docDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                        return ResponseBaseModel.CreateError(msg);
                    }

                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                            .ExecuteUpdateAsync(set => set
                                .SetProperty(p => p.Quantity, p => p.Quantity + req.Quantity), cancellationToken: token);
                }
            }

            if (lockers.Count != 0)
                context.RemoveRange(lockers);

            await context.SaveChangesAsync(token);
        }

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
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        await context.RowsOrdersRetails.Where(x => x.Id == rowId).ExecuteDeleteAsync(cancellationToken: token);
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