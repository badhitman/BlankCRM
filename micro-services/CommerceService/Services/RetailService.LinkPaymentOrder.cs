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
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<PaymentOrderRetailLinkModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new() {
                     TypeMessage = MessagesTypesEnum.Error,
                     Text = "req.Payload is null" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.Payload.OrderDocument = null;
        req.Payload.PaymentDocument = null;
        req.Payload.Name = req.Payload.Name?.Trim();

        await context.PaymentsOrdersLinks.AddAsync(req.Payload, token);
        await context.SaveChangesAsync(token);

        return new() { Response = req.Payload.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkDocumentAsync(TAuthRequestStandardModel<PaymentOrderRetailLinkModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        req.Payload.Name = req.Payload.Name?.Trim();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.PaymentsOrdersLinks
            .Where(x => x.Id == req.Payload.Id || (req.Payload.OrderDocumentId == x.OrderDocumentId && req.Payload.PaymentDocumentId == x.PaymentDocumentId))
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.AmountPayment, req.Payload.AmountPayment), cancellationToken: token);
        await context.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentOrderRetailLinkModelDB> q = context.PaymentsOrdersLinks.AsQueryable();

        bool
            forOrders = req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0,
            forPayments = req.Payload?.PaymentsIds is not null && req.Payload.PaymentsIds.Length != 0;

        if (forOrders)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (forPayments)
            q = q.Where(x => req.Payload!.PaymentsIds!.Contains(x.PaymentDocumentId));

        IQueryable<PaymentOrderRetailLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.PaymentDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<PaymentOrderRetailLinkModelDB> res = await pq
            .Include(x => x.PaymentDocument!)
            .ThenInclude(x => x.Wallet)
            .Include(x => x.OrderDocument!)
            .ThenInclude(x => x.Rows!)
            .ThenInclude(x => x.Offer)
            .ToListAsync(cancellationToken: token);

        if (forOrders != forPayments)
            foreach (PaymentOrderRetailLinkModelDB row in res.Where(x => x.AmountPayment <= 0))
            {
                if (forOrders)
                {
                    row.AmountPayment = row.PaymentDocument is null
                        ? 0
                        : row.PaymentDocument.Amount;
                }
                else if (forPayments)
                {
                    row.AmountPayment = row.OrderDocument?.Rows is null || row.OrderDocument.Rows.Count == 0
                        ? 0
                        : row.OrderDocument.Rows.Sum(x => x.Amount);
                }

                if (row.AmountPayment != 0)
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
    public async Task<TResponseModel<PaymentOrderRetailLinkModelDB[]>> PaymentsOrdersDocumentsLinksGetAsync(int[] req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.PaymentsOrdersLinks
                .Include(x => x.OrderDocument)
                .Include(x => x.PaymentDocument)
                .Where(x => req.Contains(x.Id))
                .ToArrayAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(OrderPaymentModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int res = await context.PaymentsOrdersLinks
             .Where(x => x.OrderDocumentId == req.OrderId && x.PaymentDocumentId == req.PaymentId)
             .ExecuteDeleteAsync(cancellationToken: token);

        return res == 0
            ? ResponseBaseModel.CreateInfo("Объект уже удалён")
            : ResponseBaseModel.CreateSuccess("Удалено");
    }
}