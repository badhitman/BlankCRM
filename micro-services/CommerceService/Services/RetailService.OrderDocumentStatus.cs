////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;

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

        DocumentRetailModelDB orderDb = await context.OrdersRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.OrderDocumentId, cancellationToken: token);

        StatusesDocumentsEnum? _oldStatus = orderDb.StatusDocument;

        req.DateOperation = req.DateOperation.SetKindUtc();
        req.OrderDocument = null;
        req.Name = req.Name.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await context.OrdersStatusesRetails.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        loggerRepo.LogInformation($"Для заказа (розница) #{req.OrderDocumentId} добавлен статус: [{req.DateOperation}] {req.StatusDocument}");

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        if (orderDb.Rows is null || orderDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return new() { Response = req.Id };
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

        StatusesDocumentsEnum _newStatus = await context.OrdersStatusesRetails
            .Where(y => y.OrderDocumentId == req.OrderDocumentId)
            .OrderByDescending(z => z.DateOperation)
            .Select(s => s.StatusDocument)
            .FirstAsync(cancellationToken: token);

        if ((ignoreStatuses.Contains(_newStatus) && ignoreStatuses.Contains(_oldStatus)) || (!ignoreStatuses.Contains(_newStatus) && !ignoreStatuses.Contains(_oldStatus)))
        {
            await transaction.CommitAsync(token);
            return new() { Response = req.Id };
        }

        List<OfferAvailabilityModelDB> offerAvailabilityDB = await context
           .OffersAvailability
           .Where(x => _offersIds.Contains(x.OfferId))
           .ToListAsync(cancellationToken: token);

        OfferAvailabilityModelDB? regOfferAv;


        if (!ignoreStatuses.Contains(_newStatus))
        {
            foreach (RowOfRetailOrderDocumentModelDB row in orderDb.Rows)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }
        }
        else if (!ignoreStatuses.Contains(_oldStatus))
        {
            foreach (RowOfRetailOrderDocumentModelDB row in orderDb.Rows)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }
        }

        await transaction.CommitAsync(token);

        return new() { Response = req.Id };
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
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

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
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        if (orderDb.Rows is null || orderDb.Rows.Count == 0)
        {
            await transaction.CommitAsync(token);
            return ResponseBaseModel.CreateSuccess("Ok");
        }

        StatusesDocumentsEnum _newStatus = await context.OrdersStatusesRetails
            .Where(y => y.OrderDocumentId == req.OrderDocumentId)
            .OrderByDescending(z => z.DateOperation)
            .Select(s => s.StatusDocument)
            .FirstAsync(cancellationToken: token);

        if ((ignoreStatuses.Contains(_newStatus) && ignoreStatuses.Contains(_oldStatus)) || (!ignoreStatuses.Contains(_newStatus) && !ignoreStatuses.Contains(_oldStatus)))
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

        OfferAvailabilityModelDB? regOfferAv;

        if (!ignoreStatuses.Contains(_newStatus))
        {
            foreach (RowOfRetailOrderDocumentModelDB row in orderDb.Rows)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }
        }
        else if (!ignoreStatuses.Contains(_oldStatus))
        {
            foreach (RowOfRetailOrderDocumentModelDB row in orderDb.Rows)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }
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
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await q.ExecuteDeleteAsync(cancellationToken: token);

        loggerRepo.LogInformation($"Для заказа (розница) #{orderDb.Id} удалён статус #{statusId}");

        await context.OrdersRetail
            .Where(x => x.Id == orderDb.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Version, Guid.NewGuid()).SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == orderDb.Id).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        StatusesDocumentsEnum _newStatus = await context.OrdersStatusesRetails
            .Where(y => y.OrderDocumentId == orderDb.Id)
            .OrderByDescending(z => z.DateOperation)
            .Select(s => s.StatusDocument)
            .FirstAsync(cancellationToken: token);

        if ((ignoreStatuses.Contains(_newStatus) && ignoreStatuses.Contains(_oldStatus)) || (!ignoreStatuses.Contains(_newStatus) && !ignoreStatuses.Contains(_oldStatus)))
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

        OfferAvailabilityModelDB? regOfferAv;

        if (!ignoreStatuses.Contains(_newStatus))
        {
            foreach (RowOfRetailOrderDocumentModelDB row in orderDb.Rows)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }
        }
        else if (!ignoreStatuses.Contains(_oldStatus))
        {
            foreach (RowOfRetailOrderDocumentModelDB row in orderDb.Rows!)
            {
                if (res_WarehouseReserveForRetailOrder.Response == true)
                {

                }
                else
                {

                }
            }
        }

        await transaction.CommitAsync(cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Строка-статус успешно удалена");
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