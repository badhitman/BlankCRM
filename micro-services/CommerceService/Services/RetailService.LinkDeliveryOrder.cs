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
    public async Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<RetailOrderDeliveryLinkModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (await context.OrdersDeliveriesLinks.AnyAsync(x => x.DeliveryDocumentId == req.Payload.DeliveryDocumentId && x.OrderDocumentId == req.Payload.OrderDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Warning, Text = "Документ уже добавлен" }] };

        req.Payload.OrderDocument = null;
        req.Payload.DeliveryDocument = null;

        await context.OrdersDeliveriesLinks.AddAsync(req.Payload, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Payload.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<RetailOrderDeliveryLinkModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.OrdersDeliveriesLinks
            .Where(x => x.Id == req.Payload.Id || (req.Payload.OrderDocumentId == x.OrderDocumentId && req.Payload.DeliveryDocumentId == x.DeliveryDocumentId))
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.WeightShipping, req.Payload.WeightShipping), cancellationToken: token);
        await context.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RetailOrderDeliveryLinkModelDB> q = context.OrdersDeliveriesLinks.AsQueryable();

        bool
            forOrders = req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0,
            forDeliveries = req.Payload?.DeliveriesIds is not null && req.Payload.DeliveriesIds.Length != 0;

        if (forOrders)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (forDeliveries)
            q = q.Where(x => req.Payload!.DeliveriesIds!.Contains(x.DeliveryDocumentId));

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => context.OrdersRetail.Any(y => x.OrderDocumentId == y.Id && y.ExternalDocumentId != null && y.ExternalDocumentId.Contains(req.FindQuery)));

        IQueryable<RetailOrderDeliveryLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.DeliveryDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<RetailOrderDeliveryLinkModelDB> res = (forOrders && forDeliveries) || (!forOrders && !forDeliveries)
                            ? await pq.Include(x => x.DeliveryDocument).Include(x => x.OrderDocument).ToListAsync(cancellationToken: token)
                            : forOrders
                                ? await pq.Include(x => x.DeliveryDocument!).ThenInclude(x => x.Rows!).ThenInclude(x => x.Offer).ToListAsync(cancellationToken: token)
                                : await pq.Include(x => x.OrderDocument!).ThenInclude(x => x.Rows!).ThenInclude(x => x.Offer).ToListAsync(cancellationToken: token);

        if (forOrders != forDeliveries)
            foreach (RetailOrderDeliveryLinkModelDB row in res.Where(x => x.WeightShipping <= 0))
            {
                if (forOrders)
                {
                    row.WeightShipping = row.DeliveryDocument?.Rows is null || row.DeliveryDocument.Rows.Count == 0
                        ? 0
                        : row.DeliveryDocument.Rows.Sum(x => x.WeightOffer);
                }
                else if (forDeliveries)
                {
                    row.WeightShipping = row.OrderDocument?.Rows is null || row.OrderDocument.Rows.Count == 0
                        ? 0
                        : row.OrderDocument.Rows.Sum(x => x.WeightOffer);
                }

                if (row.WeightShipping != 0)
                {
                    context.Update(row);
                    await context.SaveChangesAsync(token);
                }
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

    /// <inheritdoc/>
    public async Task<TResponseModel<RetailOrderDeliveryLinkModelDB[]>> DeliveriesOrdersLinksDocumentsReadAsync(int[] req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.OrdersDeliveriesLinks
             .Where(x => req.Contains(x.Id))
             .Include(x => x.DeliveryDocument)
             .Include(x => x.OrderDocument)
             .ToArrayAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>> TotalWeightOrdersDocumentsLinksAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.DeliveryDocumentId <= 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Не указан документ доставки" }] };

        if (!await context.OrdersDeliveriesLinks.AnyAsync(x => x.DeliveryDocumentId == req.DeliveryDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Info, Text = "Документ доставки без заказов" }] };

        return new() { Response = await context.OrdersDeliveriesLinks.Where(x => x.DeliveryDocumentId == req.DeliveryDocumentId).SumAsync(x => x.WeightShipping, cancellationToken: token) };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(TAuthRequestStandardModel<OrderDeliveryModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int res = await context.OrdersDeliveriesLinks
             .Where(x => x.OrderDocumentId == req.Payload.OrderId && x.DeliveryDocumentId == req.Payload.DeliveryId)
             .ExecuteDeleteAsync(cancellationToken: token);

        return res == 0
            ? ResponseBaseModel.CreateInfo("Объект уже удалён")
            : ResponseBaseModel.CreateSuccess("Удалено");
    }
}