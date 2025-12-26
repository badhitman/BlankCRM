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
public partial class RetailService(IIdentityTransmission identityRepo,
    ILogger<RetailService> loggerRepo,
    IKladrNavigationService kladrRepo,
    IParametersStorageTransmission StorageTransmissionRepo,
    IDbContextFactory<CommerceContext> commerceDbFactory) : IRetailService
{
    /// <summary>
    /// Статусы документов, которые не участвуют в оборотах
    /// </summary>
    /// <remarks>
    /// Движения остатков на складах
    /// </remarks>
    static readonly StatusesDocumentsEnum?[] ignoreStatuses = [StatusesDocumentsEnum.Canceled, null];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfOrdersReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DocumentRetailModelDB> q = context.OrdersRetail.AsQueryable();

        if (req.Payload?.EqualsSumFilter != true)
            q = context.OrdersRetail
                .Where(x => context.RowsOrdersRetails.Where(y => y.OrderId == x.Id).Sum(y => y.Amount) == (context.PaymentsOrdersLinks.Where(y => y.OrderDocumentId == x.Id && context.PaymentsRetailDocuments.Any(z => z.StatusPayment == PaymentsRetailStatusesEnum.Paid && z.Id == y.PaymentDocumentId)).Sum(y => y.AmountPayment) + context.ConversionsOrdersLinksRetail.Where(y => y.OrderDocumentId == x.Id && context.ConversionsDocumentsWalletsRetail.Any(z => z.Id == y.ConversionDocumentId && !z.IsDisabled)).Sum(y => y.AmountPayment)))
                .AsQueryable();

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Count != 0)
        {
            bool _unsetChecked = req.Payload.StatusesFilter.Contains(null);
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusDocument) || (_unsetChecked && x.StatusDocument == 0));
        }

        if (req.Payload is not null && req.Payload.NumWeekOfYear > 0)
            q = q.Where(x => x.NumWeekOfYear == req.Payload.NumWeekOfYear);

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        IQueryable<RowOfRetailOrderDocumentModelDB>? qr = context.RowsOrdersRetails
           .Where(x => q.Any(y => y.Id == x.OrderId))
           .AsQueryable();

        var _fq = from p in qr
                  group p by p.OfferId
                  into g
                  select new { OfferId = g.Key, Amount = g.Sum(x => x.Amount), Counter = g.Sum(x => x.Quantity) };

        var oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => _fq.OrderBy(x => x.Amount),
            DirectionsEnum.Down => _fq.OrderByDescending(x => x.Amount),
            _ => _fq.OrderBy(x => x.OfferId)
        };

        var pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        var res = await pq.ToListAsync(cancellationToken: token);
        if (res.Count == 0)
            return new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = 0,
                Response = []
            };

        int[] offersIds = [.. res.Select(x => x.OfferId)];
        OfferModelDB[] offersDb = await context.Offers.Where(x => offersIds.Contains(x.Id)).ToArrayAsync(cancellationToken: token);

        OffersRetailReportRowModel getObject(decimal offerId, decimal amountSum, decimal countSum)
        {
            return new()
            {
                Sum = amountSum,
                Count = countSum,
                Offer = offersDb.First(x => x.Id == offerId)
            };
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await _fq.CountAsync(cancellationToken: token),
            Response = [.. res.Select(x => getObject(x.OfferId, x.Amount, x.Counter))],
        };
    }

    /// <inheritdoc/>
    public async Task<MainReportResponseModel> GetMainReportAsync(MainReportRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<DocumentRetailModelDB> q = context.OrdersRetail
            .Where(x => x.StatusDocument == StatusesDocumentsEnum.Done)
            .AsQueryable();

        if (req.NumWeekOfYear > 0)
            q = q.Where(x => x.NumWeekOfYear == req.NumWeekOfYear);

        if (req.Start is not null && req.Start != default)
            q = q.Where(x => x.DateDocument >= req.Start.SetKindUtc());

        if (req.End is not null && req.End != default)
        {
            req.End = req.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.End);
        }

        IQueryable<PaymentOrderRetailLinkModelDB> qpo = context.PaymentsOrdersLinks
            .Where(x => x.PaymentDocument!.StatusPayment == PaymentsRetailStatusesEnum.Paid && q.Any(y => y.Id == x.OrderDocumentId));

        IQueryable<ConversionOrderRetailLinkModelDB> qco = context.ConversionsOrdersLinksRetail
            .Where(x => !x.ConversionDocument!.IsDisabled && q.Any(y => y.Id == x.OrderDocumentId));

        return new()
        {
            DoneOrdersCount = await q.CountAsync(cancellationToken: token),
            DoneOrdersSumAmount = await context.RowsOrdersRetails.Where(x => q.Any(y => y.Id == x.OrderId)).SumAsync(x => x.Amount, cancellationToken: token),

            PaidOnSitePaymentsSumAmount = await qpo.Where(x => x.PaymentDocument!.TypePayment == PaymentsRetailTypesEnum.OnSite).SumAsync(x => x.AmountPayment, cancellationToken: token),
            PaidOnSitePaymentsCount = await qpo.Where(x => x.PaymentDocument!.TypePayment == PaymentsRetailTypesEnum.OnSite).CountAsync(token),

            PaidNoSitePaymentsSumAmount = await qpo.Where(x => x.PaymentDocument!.TypePayment != PaymentsRetailTypesEnum.OnSite).SumAsync(x => x.AmountPayment, cancellationToken: token),
            PaidNoSitePaymentsCount = await qpo.Where(x => x.PaymentDocument!.TypePayment != PaymentsRetailTypesEnum.OnSite).CountAsync(cancellationToken: token),

            ConversionsSumAmount = await qco.SumAsync(x => x.AmountPayment, cancellationToken: token),
            ConversionsCount = await qco.CountAsync(token),
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailReportRowModel>> FinancialsReportRetailAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        //IQueryable<PaymentOrderRetailLinkModelDB> qp = context.PaymentsOrdersLinks
        //    .Where(x => x.PaymentDocument!.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        //    .Where(x => x.OrderDocument!.StatusDocument == StatusesDocumentsEnum.Done);

        //IQueryable<ConversionOrderRetailLinkModelDB> qc = context.ConversionsOrdersLinksRetail
        //    .Where(x => !x.ConversionDocument!.IsDisabled)
        //    .Where(x => x.OrderDocument!.StatusDocument == StatusesDocumentsEnum.Done);

        //if (req.Payload?.FilterIdentityIds is not null && req.Payload.FilterIdentityIds.Length != 0)
        //{
        //    qp = qp.Where(x => context.WalletsRetail.Any(y => y.Id == x.PaymentDocument!.WalletId && req.Payload.FilterIdentityIds.Contains(y.UserIdentityId)));

        //    qc = from doc in qc
        //         join sender in context.WalletsRetail on doc.ConversionDocument!.FromWalletId equals sender.Id
        //         join recipient in context.WalletsRetail on doc.ConversionDocument!.ToWalletId equals recipient.Id

        //         where req.Payload.FilterIdentityIds.Length == 0 || req.Payload.FilterIdentityIds.Contains(sender.UserIdentityId) || req.Payload.FilterIdentityIds.Contains(recipient.UserIdentityId)

        //         select doc;
        //}

        //bool conversionCheck =
        //    req.Payload?.TypesFilter is null ||
        //    req.Payload.TypesFilter.Count == 0 ||
        //    req.Payload.TypesFilter.Contains(null);

        //bool paymentsCheck =
        //    req.Payload?.TypesFilter is null ||
        //    req.Payload.TypesFilter.Count == 0 ||
        //    req.Payload.TypesFilter.Any(x => x is not null);

        //if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Any(x => x is not null))
        //{
        //    req.Payload.TypesFilter.RemoveAll(x => x is null);
        //    qp = qp.Where(x => req.Payload.TypesFilter.Contains(x.PaymentDocument!.TypePayment));
        //}

        //if (req.Payload is not null && req.Payload.NumWeekOfYear > 0)
        //{
        //    qp = qp.Where(x => x.OrderDocument!.NumWeekOfYear == req.Payload.NumWeekOfYear);
        //    qc = qc.Where(x => x.OrderDocument!.NumWeekOfYear == req.Payload.NumWeekOfYear);
        //}

        //if (req.Payload?.Start is not null && req.Payload.Start != default)
        //{
        //    qp = qp.Where(x => x.PaymentDocument!.DatePayment >= req.Payload.Start.SetKindUtc());
        //    qc = qc.Where(x => x.ConversionDocument!.DateDocument >= req.Payload.Start.SetKindUtc());
        //}

        //if (req.Payload?.End is not null && req.Payload.End != default)
        //{
        //    req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
        //    qp = qp.Where(x => x.PaymentDocument!.DatePayment <= req.Payload.End);
        //    qc = qc.Where(x => x.ConversionDocument!.DateDocument <= req.Payload.End);
        //}

        //var _qp1 = qp.Select(x => new { x.PaymentDocument!.WalletId, Amount = x.AmountPayment });
        //var _qp2 = qc.Select(x => new { WalletId = x.ConversionDocument!.FromWalletId, Amount = -x.ConversionDocument.FromWalletSum });
        //var _qp3 = qc.Select(x => new { WalletId = x.ConversionDocument!.ToWalletId, Amount = x.ConversionDocument.ToWalletSum });

        //var unionQuery = conversionCheck ? _qp1.Union(_qp2).Union(_qp3) : _qp1;
        //if (!paymentsCheck)
        //    unionQuery = _qp2.Union(_qp3);

        //var _fq = from p in unionQuery
        //          group p by p.WalletId
        //          into g
        //          select new { WalletId = g.Key, Amount = g.Sum(x => x.Amount) };

        //var _oq = req.SortingDirection == DirectionsEnum.Up
        //    ? _fq.OrderBy(x => x.Amount)
        //    : _fq.OrderByDescending(x => x.Amount);

        //var pq = _oq
        //    .Skip(req.PageNum * req.PageSize)
        //    .Take(req.PageSize);

        //var res = await pq.ToListAsync(cancellationToken: token);

        //int[] _walletsIds = [.. res.Select(x => x.WalletId).Distinct()];
        //WalletRetailModelDB[] getWalletsDb = await context.WalletsRetail
        //    .Where(x => _walletsIds.Contains(x.Id))
        //    .Include(x => x.WalletType)
        //    .ToArrayAsync(cancellationToken: token);

        //string[] usersIds = [.. getWalletsDb.Select(x => x.UserIdentityId).Distinct()];
        //TResponseModel<UserInfoModel[]> getUsers = await identityRepo.GetUsersOfIdentityAsync(usersIds, token);

        //WalletRetailReportRowModel getObject(int walletId, decimal amount)
        //{
        //    WalletRetailModelDB _w = getWalletsDb.First(y => y.Id == walletId);
        //    return new()
        //    {
        //        Amount = amount,
        //        Wallet = _w,
        //        User = getUsers.Response!.First(x => x.UserId == _w.UserIdentityId)
        //    };
        //}

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            //TotalRowsCount = await _fq.CountAsync(cancellationToken: token),
            //Response = [.. res.Select(x => getObject(x.WalletId, x.Amount))]
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfDeliveriesReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryDocumentRetailModelDB> q = context.DeliveryDocumentsRetail.AsQueryable();

        if (req.Payload?.EqualsSumFilter != true)
            q = context.DeliveryDocumentsRetail
                .Where(x => context.RowsDeliveryDocumentsRetail.Where(y => y.DocumentId == x.Id).Sum(y => y.WeightOffer) == (context.OrdersDeliveriesLinks.Where(y => y.OrderDocumentId == x.Id && context.DeliveryDocumentsRetail.Any(z => z.DeliveryStatus == DeliveryStatusesEnum.Delivered && z.Id == y.DeliveryDocumentId)).Sum(y => y.WeightShipping)))
                .AsQueryable();

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Count != 0)
        {
            bool _unsetChecked = req.Payload.StatusesFilter.Contains(null);
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.DeliveryStatus) || (_unsetChecked && x.DeliveryStatus == 0));
        }

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.CreatedAtUTC >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.CreatedAtUTC <= req.Payload.End);
        }

        IQueryable<RowOfDeliveryRetailDocumentModelDB> qr = context.RowsDeliveryDocumentsRetail.Where(x => q.Any(y => y.Id == x.DocumentId)).AsQueryable();


        var _fq = from p in qr
                  group p by p.OfferId
                  into g
                  select new { OfferId = g.Key, Weight = g.Sum(x => x.WeightOffer), Counter = g.Sum(x => x.Quantity) };

        var oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => _fq.OrderBy(x => x.Weight),
            DirectionsEnum.Down => _fq.OrderByDescending(x => x.Weight),
            _ => _fq.OrderBy(x => x.OfferId)
        };

        var pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        var res = await pq.ToListAsync(cancellationToken: token);
        if (res.Count == 0)
            return new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = 0,
                Response = []
            };

        int[] offersIds = [.. res.Select(x => x.OfferId)];
        OfferModelDB[] offersDb = await context.Offers.Where(x => offersIds.Contains(x.Id)).ToArrayAsync(cancellationToken: token);

        OffersRetailReportRowModel getObject(decimal offerId, decimal weightSum, decimal countSum)
        {
            return new()
            {
                Sum = weightSum,
                Count = countSum,
                Offer = offersDb.First(x => x.Id == offerId)
            };
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await _fq.CountAsync(cancellationToken: token),
            Response = [.. res.Select(x => getObject(x.OfferId, x.Weight, x.Counter))],
        };
    }

    /// <inheritdoc/>
    public async Task<PeriodBaseModel> AboutPeriodAsync(object? req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<DateTime>
            q1 = context.OrdersRetail.Select(x => x.DateDocument),
            q2 = context.PaymentsRetailDocuments.Select(x => x.DatePayment),
            q3 = context.DeliveryDocumentsRetail.Select(x => x.CreatedAtUTC),
            q4 = context.ConversionsDocumentsWalletsRetail.Select(x => x.DateDocument);

        IQueryable<DateTime> q = q1.Union(q2).Union(q3).Union(q4);
        if (!await q.AnyAsync(cancellationToken: token))
            return new();

        return new()
        {
            Start = await q.MinAsync(cancellationToken: token),
            End = await q.MaxAsync(cancellationToken: token),
        };
    }
}