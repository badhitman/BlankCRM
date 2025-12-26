////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.DateOperation = req.DateOperation.SetKindUtc();
        req.DeliveryDocument = null;
        req.Name = req.Name.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        await context.DeliveriesStatusesDocumentsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveriesStatusesDocumentsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.DeliveryStatus, req.DeliveryStatus)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
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

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryStatusDocumentAsync(int statusId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB> q = context.DeliveriesStatusesDocumentsRetail.Where(x => x.Id == statusId);
        int deliveryDocumentId = await q.Select(x => x.DeliveryDocumentId).FirstAsync(cancellationToken: token);

        await q.ExecuteDeleteAsync(cancellationToken: token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == deliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == deliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Строка-статус успешно удалена");
    }
}