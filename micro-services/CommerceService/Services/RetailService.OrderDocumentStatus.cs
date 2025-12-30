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
    public async Task<TResponseModel<int>> CreateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DocumentRetailModelDB docDb = await context.OrdersRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.OrderDocumentId, cancellationToken: token);

        StatusesDocumentsEnum? _oldStatus = docDb.StatusDocument;

        req.DateOperation = req.DateOperation.SetKindUtc();
        req.OrderDocument = null;
        req.Name = req.Name.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await context.OrdersStatusesRetails.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        loggerRepo.LogInformation($"Для заказа (розница) #{req.OrderDocumentId} добавлен статус: [{req.DateOperation}] {req.StatusDocument}");

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        if (docDb.Rows is null || docDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return new() { Response = req.Id };
        }

        int[] _offersIds = [.. docDb.Rows.Select(x => x.OfferId)];
        List<LockTransactionModelDB> lockers = [.._offersIds.Select(x=> new LockTransactionModelDB()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = x,
            LockerAreaId = docDb.WarehouseId,
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
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}{ex.Message}" }] };
        }

        StatusesDocumentsEnum _newStatus = await context.OrdersStatusesRetails
            .Where(y => y.OrderDocumentId == req.OrderDocumentId)
            .OrderByDescending(z => z.DateOperation)
            .ThenByDescending(os => os.Id)
            .Select(s => s.StatusDocument)
            .FirstAsync(cancellationToken: token);

        if ((offOrdersStatuses.Contains(_newStatus) && offOrdersStatuses.Contains(_oldStatus)) || (!offOrdersStatuses.Contains(_newStatus) && !offOrdersStatuses.Contains(_oldStatus)))
        {
            await transaction.CommitAsync(token);
            return new() { Response = req.Id };
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => _offersIds.Contains(x.OfferId))
           .ToListAsync(cancellationToken: token);

        ResponseBaseModel res = await DoIt(context, transaction, docDb.Rows, res_WarehouseReserveForRetailOrder.Response == true, !offOrdersStatuses.Contains(_newStatus), offerAvailabilityDB, docDb, token);
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
        return new() { Response = req.Id, Messages = [new() { TypeMessage = MessagesTypesEnum.Success, Text = (_oldStatus == _newStatus ? "конечный статус документа без изменений" : "статус документа изменён") }] };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DocumentRetailModelDB orderDb = await context.OrdersRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.OrderDocumentId, cancellationToken: token);
        StatusesDocumentsEnum? _oldStatus = orderDb.StatusDocument;
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        req.Name = req.Name.Trim();
        await context.OrdersStatusesRetails
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.StatusDocument, req.StatusDocument)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        loggerRepo.LogInformation($"Для заказа (розница) #{req.OrderDocumentId} обновлён статус: [{req.DateOperation}] {req.StatusDocument}");

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        if (orderDb.Rows is null || orderDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        StatusesDocumentsEnum _newStatus = await context.OrdersStatusesRetails
            .Where(y => y.OrderDocumentId == req.OrderDocumentId)
            .OrderByDescending(z => z.DateOperation)
            .ThenByDescending(os => os.Id)
            .Select(s => s.StatusDocument)
            .FirstAsync(cancellationToken: token);

        if ((offOrdersStatuses.Contains(_newStatus) && offOrdersStatuses.Contains(_oldStatus)) || (!offOrdersStatuses.Contains(_newStatus) && !offOrdersStatuses.Contains(_oldStatus)))
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        int[] _offersIds = [.. orderDb.Rows.Select(x => x.OfferId)];
        List<LockTransactionModelDB> lockers = [.._offersIds.Select(x=> new LockTransactionModelDB()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = x,
            LockerAreaId = orderDb.WarehouseId,
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
            loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}{ex.Message}" }] };
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => _offersIds.Contains(x.OfferId))
           .ToListAsync(cancellationToken: token);

        ResponseBaseModel res = await DoIt(context, transaction, orderDb.Rows, res_WarehouseReserveForRetailOrder.Response == true, !offOrdersStatuses.Contains(_newStatus), offerAvailabilityDB, orderDb, token);
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
    public async Task<ResponseBaseModel> DeleteOrderStatusDocumentAsync(int statusId, CancellationToken token = default)
    {
        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderStatusRetailDocumentModelDB> q = context.OrdersStatusesRetails.Where(x => x.Id == statusId);
        DocumentRetailModelDB orderDb = await q.Select(x => x.OrderDocument!)
            .Include(x => x.Rows)
            .FirstAsync(cancellationToken: token) ?? throw new Exception($"{nameof(DeleteOrderStatusDocumentAsync)}");
        StatusesDocumentsEnum? _oldStatus = orderDb.StatusDocument;
        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await q.ExecuteDeleteAsync(cancellationToken: token);

        loggerRepo.LogInformation($"Для заказа (розница) #{orderDb.Id} удалён статус #{statusId}");

        await context.OrdersRetail
            .Where(x => x.Id == orderDb.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == orderDb.Id).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        StatusesDocumentsEnum _newStatus = await context.OrdersStatusesRetails
            .Where(y => y.OrderDocumentId == orderDb.Id)
            .OrderByDescending(z => z.DateOperation)
            .ThenByDescending(os => os.Id)
            .Select(s => s.StatusDocument)
            .FirstAsync(cancellationToken: token);

        if ((offOrdersStatuses.Contains(_newStatus) && offOrdersStatuses.Contains(_oldStatus)) || (!offOrdersStatuses.Contains(_newStatus) && !offOrdersStatuses.Contains(_oldStatus)))
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        if (orderDb.Rows is null || orderDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        int[] _offersIds = [.. orderDb.Rows.Select(x => x.OfferId)];
        List<LockTransactionModelDB> lockers = [.._offersIds.Select(x=> new LockTransactionModelDB()
        {
            LockerName = nameof(OfferAvailabilityModelDB),
            LockerId = x,
            LockerAreaId = orderDb.WarehouseId,
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

        ResponseBaseModel res = await DoIt(context, transaction, orderDb.Rows, res_WarehouseReserveForRetailOrder.Response == true, !offOrdersStatuses.Contains(_newStatus), offerAvailabilityDB, orderDb, token);
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

        await transaction.CommitAsync(cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Строка-статус успешно удалена");
    }

    /// <inheritdoc/>
    async Task<ResponseBaseModel> DoIt(CommerceContext context, IDbContextTransaction transaction, List<RowOfRetailOrderDocumentModelDB> rows, bool reserveForRetailOrder, bool isEnableDocument, List<OfferAvailabilityModelDB> offerAvailabilityDB, DocumentRetailModelDB orderDb, CancellationToken token = default)
    {
        foreach (RowOfRetailOrderDocumentModelDB row in rows)
        {
            OfferAvailabilityModelDB? regOfferAv = offerAvailabilityDB
                .FirstOrDefault(x => x.OfferId == row.OfferId && x.WarehouseId == orderDb.WarehouseId);
            string msg;
            if (isEnableDocument) // (ON) включение
            {
                if (reserveForRetailOrder) // зачислить остаток на склад
                {
                    if (regOfferAv is null)
                    {
                        regOfferAv = new()
                        {
                            WarehouseId = orderDb.WarehouseId,
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
                else // списать остаток со склада
                {
                    if (regOfferAv is null)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{orderDb.WarehouseId} отсутствует офер #{row.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                        return ResponseBaseModel.CreateError(msg);
                    }
                    else if (regOfferAv.Quantity < row.Quantity)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{orderDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                        return ResponseBaseModel.CreateError(msg);
                    }

                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - row.Quantity), cancellationToken: token);
                }
            }
            else // (OFF) выключение
            {
                if (reserveForRetailOrder) // списать остаток со склада
                {
                    if (regOfferAv is null)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{orderDb.WarehouseId} отсутствует офер #{row.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                        return ResponseBaseModel.CreateError(msg);
                    }
                    else if (regOfferAv.Quantity < row.Quantity)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"На складе #{orderDb.WarehouseId} отсутствует офер #{regOfferAv.OfferId}";
                        loggerRepo.LogError($"{msg}: {JsonConvert.SerializeObject(row, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

                        return ResponseBaseModel.CreateError(msg);
                    }

                    await context.OffersAvailability
                        .Where(x => x.Id == regOfferAv.Id)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - row.Quantity), cancellationToken: token);
                }
                else // зачислить остаток на склад
                {
                    if (regOfferAv is null)
                    {
                        regOfferAv = new()
                        {
                            WarehouseId = orderDb.WarehouseId,
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
        }
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            string msg = "req.Payload is null";
            loggerRepo.LogError(msg);
            return new()
            {
                Status = new()
                {
                    Messages = [new()
                    {
                        TypeMessage = MessagesTypesEnum.Error,
                        Text = msg
                    }]
                }
            };
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderStatusRetailDocumentModelDB> q = context.OrdersStatusesRetails
            .Where(x => x.OrderDocumentId == req.Payload.OrderDocumentId).AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        IQueryable<OrderStatusRetailDocumentModelDB>? pq = q
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