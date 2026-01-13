////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DeliveryDocumentRetailModelDB docDb = await context.DeliveryDocumentsRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Payload.DeliveryDocumentId, cancellationToken: token);

        DeliveryStatusesEnum? _oldStatus = docDb.DeliveryStatus;

        req.Payload.DateOperation = req.Payload.DateOperation.SetKindUtc();
        req.Payload.DeliveryDocument = null;
        req.Payload.Name = req.Payload.Name.Trim();
        req.Payload.CreatedAtUTC = DateTime.UtcNow;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await context.DeliveriesStatusesDocumentsRetail.AddAsync(req.Payload, token);
        await context.SaveChangesAsync(token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.Payload.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.Payload.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        if (docDb.Rows is null || docDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return new() { Response = req.Payload.Id };
        }

        int[] _offersIds = [.. docDb.Rows.Select(x => x.OfferId)];
        List<LockTransactionModelDB> lockers = [.._offersIds.Select(x=> new LockTransactionModelDB()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = x,
            LockerAreaId = docDb.WarehouseId,
            Marker = nameof(CreateDeliveryStatusDocumentAsync)
        })];

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

        DeliveryStatusesEnum _newStatus = await context.DeliveriesStatusesDocumentsRetail
            .Where(y => y.DeliveryDocumentId == req.Payload.DeliveryDocumentId)
            .OrderByDescending(z => z.DateOperation)
            .ThenByDescending(os => os.Id)
            .Select(s => s.DeliveryStatus)
            .FirstAsync(cancellationToken: token);

        if ((offDeliveriesStatuses.Contains(_newStatus) && offDeliveriesStatuses.Contains(_oldStatus)) || (!offDeliveriesStatuses.Contains(_newStatus) && !offDeliveriesStatuses.Contains(_oldStatus)))
        {
            if (lockers.Count != 0)
            {
                context.RemoveRange(lockers);
                await context.SaveChangesAsync(token);
            }

            await transaction.CommitAsync(token);
            return new() { Response = req.Payload.Id };
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => _offersIds.Contains(x.OfferId))
           .ToListAsync(cancellationToken: token);

        ResponseBaseModel res = await DoIt(context, transaction, docDb.Rows, !offDeliveriesStatuses.Contains(_newStatus), offerAvailabilityDB, docDb, token);
        if (!res.Success())
        {
            await transaction.RollbackAsync(token);
            return new() { Messages = res.Messages };
        }

        if (lockers.Count != 0)
        {
            context.RemoveRange(lockers);
            await context.SaveChangesAsync(token);
        }

        await transaction.CommitAsync(token);
        return new() { Response = req.Payload.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DeliveryDocumentRetailModelDB docDb = await context.DeliveryDocumentsRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Payload.DeliveryDocumentId, cancellationToken: token);

        DeliveryStatusesEnum? _oldStatus = docDb.DeliveryStatus;
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        req.Payload.Name = req.Payload.Name.Trim();
        await context.DeliveriesStatusesDocumentsRetail
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.DeliveryStatus, req.Payload.DeliveryStatus)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.Payload.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.Payload.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        if (docDb.Rows is null || docDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        DeliveryStatusesEnum _newStatus = await context.DeliveriesStatusesDocumentsRetail
            .Where(y => y.DeliveryDocumentId == req.Payload.DeliveryDocumentId)
            .OrderByDescending(z => z.DateOperation)
            .ThenByDescending(os => os.Id)
            .Select(s => s.DeliveryStatus)
            .FirstAsync(cancellationToken: token);

        if ((offDeliveriesStatuses.Contains(_newStatus) && offDeliveriesStatuses.Contains(_oldStatus)) || (!offDeliveriesStatuses.Contains(_newStatus) && !offDeliveriesStatuses.Contains(_oldStatus)))
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        int[] _offersIds = [.. docDb.Rows.Select(x => x.OfferId)];
        List<LockTransactionModelDB> lockers = [.._offersIds.Select(x=> new LockTransactionModelDB()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = x,
            LockerAreaId = docDb.WarehouseId,
            Marker = nameof(UpdateDeliveryStatusDocumentAsync)
        })];

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
           .Where(x => _offersIds.Contains(x.OfferId))
           .ToListAsync(cancellationToken: token);

        ResponseBaseModel res = await DoIt(context, transaction, docDb.Rows, !offDeliveriesStatuses.Contains(_newStatus), offerAvailabilityDB, docDb, token);
        if (!res.Success())
        {
            await transaction.RollbackAsync(token);
            return new() { Messages = res.Messages };
        }

        if (lockers.Count != 0)
        {
            context.RemoveRange(lockers);
            await context.SaveChangesAsync(token);
        }

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB>> DeleteDeliveryStatusDocumentAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        int statusId = req.Payload;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB> q = context.DeliveriesStatusesDocumentsRetail.Where(x => x.Id == statusId);
        DeliveryStatusRetailDocumentModelDB statusDb = await q
            .Include(x => x.DeliveryDocument!)
            .ThenInclude(x => x.Rows)
            .FirstAsync(cancellationToken: token);

        TResponseModel<DeliveryDocumentRetailModelDB> res = new() { Response = statusDb.DeliveryDocument! };
        DeliveryStatusesEnum? _oldStatus = res.Response.DeliveryStatus;
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        await q.ExecuteDeleteAsync(cancellationToken: token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == res.Response.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == res.Response.Id).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        DeliveryStatusesEnum _newStatus = await context.DeliveriesStatusesDocumentsRetail
            .Where(y => y.DeliveryDocumentId == res.Response.Id)
            .OrderByDescending(z => z.DateOperation)
            .ThenByDescending(os => os.Id)
            .Select(s => s.DeliveryStatus)
            .FirstAsync(cancellationToken: token);

        if ((offDeliveriesStatuses.Contains(_newStatus) && offDeliveriesStatuses.Contains(_oldStatus)) || (!offDeliveriesStatuses.Contains(_newStatus) && !offDeliveriesStatuses.Contains(_oldStatus)))
        {
            await transaction.CommitAsync(token);
            res.AddSuccess("Ok");
            return res;
        }

        if (res.Response.Rows is null || res.Response.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            res.AddSuccess("Ok");
            return res;
        }

        int[] _offersIds = [.. res.Response.Rows.Select(x => x.OfferId)];
        List<LockTransactionModelDB> lockers = [.._offersIds.Select(x=> new LockTransactionModelDB()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = x,
            LockerAreaId = res.Response.WarehouseId,
            Marker = nameof(DeleteDeliveryStatusDocumentAsync),
        })];

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
            loggerRepo.LogError(ex, msg);
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}{ex.Message}" }] };
        }
        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => _offersIds.Contains(x.OfferId))
           .ToListAsync(cancellationToken: token);

        ResponseBaseModel sRes = await DoIt(context, transaction, res.Response.Rows, !offDeliveriesStatuses.Contains(_newStatus), offerAvailabilityDB, res.Response, token);
        if (!sRes.Success())
        {
            await transaction.RollbackAsync(token);
            return new() { Messages = sRes.Messages };
        }

        if (lockers.Count != 0)
        {
            context.RemoveRange(lockers);
            await context.SaveChangesAsync(token);
        }

        res.AddSuccess("Строка-статус успешно удалена");
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DoIt(CommerceContext context, IDbContextTransaction transaction, List<RowOfDeliveryRetailDocumentModelDB> rows, bool isEnableDocument, List<OfferAvailabilityModelDB> offerAvailabilityDB, DeliveryDocumentRetailModelDB deliveryDocDb, CancellationToken token = default)
    {
        loggerRepo.LogInformation($"{nameof(deliveryDocDb)}: {JsonConvert.SerializeObject(deliveryDocDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        foreach (RowOfDeliveryRetailDocumentModelDB row in rows)
        {
            OfferAvailabilityModelDB? regOfferAv = offerAvailabilityDB
                .FirstOrDefault(x => x.OfferId == row.OfferId && x.WarehouseId == deliveryDocDb.WarehouseId);
            string msg;
            if (isEnableDocument) // (ON) включение
            {
                if (regOfferAv is null)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{deliveryDocDb.WarehouseId} отсутствует офер #{row.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return ResponseBaseModel.CreateError(msg);
                }
                else if (regOfferAv.Quantity < row.Quantity)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"На складе #{deliveryDocDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                    loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                    return ResponseBaseModel.CreateError(msg);
                }

                await context.OffersAvailability
                    .Where(x => x.Id == regOfferAv.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity - row.Quantity), cancellationToken: token);

            }
            else // (OFF) выключение
            {
                if (regOfferAv is null)
                {
                    regOfferAv = new()
                    {
                        WarehouseId = deliveryDocDb.WarehouseId,
                        NomenclatureId = row.NomenclatureId,
                        OfferId = row.OfferId,
                        Quantity = row.Quantity,
                    };
                    await context.OffersAvailability.AddAsync(regOfferAv, token);
                    await context.SaveChangesAsync(token);
                    offerAvailabilityDB.Add(regOfferAv);
                }
                else
                {
                    await context.OffersAvailability.Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Quantity, p => p.Quantity + row.Quantity), cancellationToken: token);
                }
            }
        }
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }]
                }
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB> q = context.DeliveriesStatusesDocumentsRetail
            .Where(x => x.DeliveryDocumentId == req.Payload.DeliveryDocumentId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        IQueryable<DeliveryStatusRetailDocumentModelDB>? pq = q
            .OrderBy(x => x.DateOperation)
            .ThenBy(os => os.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq.ToListAsync(cancellationToken: token)
        };
    }
}