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

        if (req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (req.Payload?.ConversionsIds is not null && req.Payload.ConversionsIds.Length != 0)
            q = q.Where(x => req.Payload!.ConversionsIds!.Contains(x.ConversionDocumentId));

        IQueryable<ConversionOrderRetailLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.ConversionDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<ConversionOrderRetailLinkModelDB> res = await pq.ToListAsync(cancellationToken: token);

        int[] ordersIds = [.. res.Select(x => x.OrderDocumentId).Distinct()];
        List<DocumentRetailModelDB> ordersDb = ordersIds.Length == 0
            ? []
            : await context.OrdersRetail
                .Where(x => ordersIds.Contains(x.Id))
                .ToListAsync(cancellationToken: token);

        int[] conversionsIds = [.. res.Select(x => x.ConversionDocumentId).Distinct()];
        List<WalletConversionRetailDocumentModelDB> conversionsDb = conversionsIds.Length == 0
            ? []
            : await context.ConversionsDocumentsWalletsRetail
                .Where(x => conversionsIds.Contains(x.Id))
                .ToListAsync(cancellationToken: token);

        int[] walletsIds = [.. conversionsDb.Select(x => x.FromWalletId).Union(conversionsDb.Select(x => x.ToWalletId)).Distinct()];
        List<WalletRetailModelDB> walletsDb = walletsIds.Length == 0
            ? []
            : await context.WalletsRetail
                .Where(x => walletsIds.Contains(x.Id))
                .ToListAsync(cancellationToken: token);

        int[] walletsTypesIds = [.. walletsDb.Select(x => x.WalletTypeId).Distinct()];
        List<WalletRetailTypeModelDB> walletsTypesDb = walletsTypesIds.Length == 0
            ? []
            : await context.WalletsRetailTypes
                .Where(x => walletsTypesIds.Contains(x.Id))
                .ToListAsync(cancellationToken: token);

        res.ForEach(link =>
        {
            link.OrderDocument = ordersDb.First(y => y.Id == link.OrderDocumentId);
            link.ConversionDocument = conversionsDb.First(y => y.Id == link.ConversionDocumentId);

            link.ConversionDocument.FromWallet = walletsDb.First(x => x.Id == link.ConversionDocument.FromWalletId);
            link.ConversionDocument.FromWallet.WalletType = walletsTypesDb.First(x => x.Id == link.ConversionDocument.FromWallet.WalletTypeId);

            link.ConversionDocument.ToWallet = walletsDb.First(x => x.Id == link.ConversionDocument.ToWalletId);
            link.ConversionDocument.ToWallet.WalletType = walletsTypesDb.First(x => x.Id == link.ConversionDocument.ToWallet.WalletTypeId);
        });

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