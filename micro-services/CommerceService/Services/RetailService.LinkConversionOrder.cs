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
    public async Task<TResponseModel<int>> CreateConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionAmountModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (await context.ConversionsOrdersLinksRetail.AnyAsync(x => x.ConversionDocumentId == req.Payload.ConversionDocumentId && x.OrderDocumentId == req.Payload.OrderDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Warning, Text = "Документ уже добавлен" }] };

        ConversionOrderRetailLinkModelDB linkDb = ConversionOrderRetailLinkModelDB.Build(req.Payload);
        await context.ConversionsOrdersLinksRetail.AddAsync(linkDb, token);
        await context.SaveChangesAsync(token);
        return new() { Response = linkDb.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionAmountModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.ConversionsOrdersLinksRetail
            .Where(x => req.Payload.OrderDocumentId == x.OrderDocumentId && req.Payload.ConversionDocumentId == x.ConversionDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.AmountPayment, req.Payload.AmountPayment), cancellationToken: token);

        await context.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksRetailAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<ConversionOrderRetailLinkModelDB> q = context.ConversionsOrdersLinksRetail.AsQueryable();

        bool
            forOrders = req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0,
            forConversions = req.Payload?.ConversionsIds is not null && req.Payload.ConversionsIds.Length != 0;

        if (forOrders)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (forConversions)
            q = q.Where(x => req.Payload!.ConversionsIds!.Contains(x.ConversionDocumentId));

        IQueryable<ConversionOrderRetailLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.ConversionDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ConversionOrderRetailLinkModelDB, DocumentRetailModelDB?>? v = pq
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.FromWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.ToWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.OrderDocument);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ConversionOrderRetailLinkModelDB, DocumentRetailModelDB?> BuildQuery()
        {
            return pq
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.FromWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.ToWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.OrderDocument);
        }

        List<ConversionOrderRetailLinkModelDB> res = await BuildQuery().ToListAsync(cancellationToken: token);

        if (forOrders != forConversions)
            foreach (ConversionOrderRetailLinkModelDB row in res.Where(x => x.AmountPayment <= 0))
            {
                if (forOrders)
                {
                    row.AmountPayment = row.ConversionDocument is null
                        ? 0
                        : row.ConversionDocument.ToWalletSum;
                }
                else if (forConversions)
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
            Response = res,
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ConversionOrderRetailLinkModelDB[]>> ConversionsOrdersDocumentsLinksReadRetailAsync(int[] req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.ConversionsOrdersLinksRetail
             .Where(x => req.Contains(x.Id))
             .Include(x => x.OrderDocument)
             .Include(x => x.ConversionDocument)
             .ToArrayAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentRetailAsync(TAuthRequestStandardModel<OrderConversionModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int res = await context.ConversionsOrdersLinksRetail
             .Where(x => x.OrderDocumentId == req.Payload.OrderDocumentId && x.ConversionDocumentId == req.Payload.ConversionDocumentId)
             .ExecuteDeleteAsync(cancellationToken: token);

        return res == 0
            ? ResponseBaseModel.CreateInfo("Объект уже удалён")
            : ResponseBaseModel.CreateSuccess("Удалено");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>> GetSumConversionsOrdersAmountsAsync(GetSumConversionsOrdersAmountsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<ConversionOrderRetailLinkModelDB> q = context.ConversionsOrdersLinksRetail.AsQueryable();

        if (req.ConversionsDocumentsIds is not null && req.ConversionsDocumentsIds.Length != 0)
            q = q.Where(x => req.ConversionsDocumentsIds.Contains(x.ConversionDocumentId));

        if (req.OrdersDocumentsIds is not null && req.OrdersDocumentsIds.Length != 0)
            q = q.Where(x => req.OrdersDocumentsIds.Contains(x.OrderDocumentId));

        return new()
        {
            Response = await q.AnyAsync(cancellationToken: token) ? await q.SumAsync(x => x.AmountPayment, cancellationToken: token) : 0
        };
    }
}