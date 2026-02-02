////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using Microsoft.EntityFrameworkCore;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(TAuthRequestStandardModel<CreatePaymentRetailDocumentRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }]
            };

        if (req.Payload.Amount == 0)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Укажите сумму платежа" }]
            };

        if (req.Payload.WalletId <= 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Не указан кошелёк" }] };

        if (req.Payload.TypePaymentId == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Не указан тип платежа" }] };

        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        WalletRetailModelDB walletDb = await context.WalletsRetail
            .Include(x => x.WalletType)
            .FirstAsync(x => x.Id == req.Payload.WalletId, cancellationToken: token);

        if (walletDb.WalletType!.IsSystem)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Зачисление на системный кошелёк невозможно" }] };

        req.Payload.Version = Guid.NewGuid();
        req.Payload.Wallet = null;
        req.Payload.PaymentSource = req.Payload.PaymentSource?.Trim();
        req.Payload.Name = req.Payload.Name.Trim();
        req.Payload.Description = req.Payload.Description?.Trim();
        req.Payload.DatePayment = req.Payload.DatePayment.SetKindUtc();
        req.Payload.CreatedAtUTC = DateTime.UtcNow;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        PaymentRetailDocumentModelDB docDb = PaymentRetailDocumentModelDB.Build(req.Payload);

        try
        {
            await context.PaymentsRetailDocuments.AddAsync(docDb, token);
            await context.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        res.Response = docDb.Id;
        res.AddSuccess($"Документ платежа/оплаты создан #{docDb.Id}");

        if (req.Payload.InjectToOrderId > 0)
        {
            TraceReceiverRecord trace = TraceReceiverRecord.Build(GlobalStaticConstantsTransmission.TransmissionQueues.CreatePaymentOrderLinkDocumentRetailReceive, req.SenderActionUserId, new PaymentOrderRetailLinkModelDB()
            {
                PaymentDocumentId = docDb.Id,
                AmountPayment = req.Payload.Amount,
                OrderDocumentId = req.Payload.InjectToOrderId,
            });

            PaymentOrderRetailLinkModelDB _paymentOrderRetailLink = new()
            {
                OrderDocumentId = req.Payload.InjectToOrderId,
                PaymentDocumentId = docDb.Id,
                AmountPayment = docDb.Amount
            };
            await context.PaymentsOrdersLinks.AddAsync(_paymentOrderRetailLink, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь оплаты/платежа #{docDb.Id} с заказом #{req.Payload.InjectToOrderId}");

            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(new TResponseModel<int>()
            {
                Response = _paymentOrderRetailLink.Id,
            }), token);
        }

        if (req.Payload.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        {
            await context.WalletsRetail
             .Where(x => x.Id == req.Payload.WalletId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Balance, p => p.Balance + req.Payload.Amount), cancellationToken: token);
        }

        await transaction.CommitAsync(token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdatePaymentDocumentAsync(TAuthRequestStandardModel<PaymentRetailDocumentModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "req.Payload is null"
                }]
            };

        if (req.Payload.Amount == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Укажите сумму платежа" }] };

        req.Payload.PaymentSource = req.Payload.PaymentSource?.Trim();
        req.Payload.Name = req.Payload.Name.Trim();
        req.Payload.Description = req.Payload.Description?.Trim();
        req.Payload.DatePayment = req.Payload.DatePayment.SetKindUtc();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        PaymentRetailDocumentModelDB paymentDb = await context.PaymentsRetailDocuments
            .Include(x => x.Wallet!)
            .ThenInclude(x => x.WalletType)
            .FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);

        if (paymentDb.Version != req.Payload.Version)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Документ ранее был кем-то изменён. Обновите документ (F5) перед его редактированием.",
                }]
            };
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        Guid _ng = Guid.NewGuid();
        if (paymentDb.Wallet?.WalletType?.IgnoreBalanceChanges == true)
        {
            await context.PaymentsRetailDocuments
                .Where(x => x.Id == req.Payload.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Name, req.Payload.Name)
                    .SetProperty(p => p.Description, req.Payload.Description)
                    .SetProperty(p => p.WalletId, req.Payload.WalletId)
                    .SetProperty(p => p.Version, _ng)
                    .SetProperty(p => p.TypePaymentId, req.Payload.TypePaymentId)
                    .SetProperty(p => p.StatusPayment, req.Payload.StatusPayment)
                    .SetProperty(p => p.PaymentSource, req.Payload.PaymentSource)
                    .SetProperty(p => p.DatePayment, req.Payload.DatePayment)
                    .SetProperty(p => p.Amount, req.Payload.Amount)
                    .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

            await transaction.CommitAsync(token);
            return new()
            {
                Response = _ng,
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Success,
                    Text = "Ok" }]
            };
        }

        if (req.Payload.StatusPayment == paymentDb.StatusPayment && req.Payload.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        {
            if (req.Payload.WalletId == paymentDb.WalletId)
            {
                decimal _deltaChange = req.Payload.Amount - paymentDb.Amount;
                if (_deltaChange < 0 && paymentDb.Wallet!.Balance < -_deltaChange)
                {
                    await transaction.RollbackAsync(token);
                    return new()
                    {
                        Messages = [new()
                        {
                            Text = $"В следствии изменения документа - сумма баланса [wallet:{paymentDb.Wallet.WalletType}] станет отрицательной",
                            TypeMessage = MessagesTypesEnum.Error }]
                    };
                }

                if (_deltaChange != 0)
                {
                    await context.WalletsRetail
                        .Where(x => x.Id == paymentDb.WalletId)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Balance, p => p.Balance + _deltaChange)
                            .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
                }
            }
            else if (paymentDb.Wallet!.Balance < paymentDb.Amount)
            {
                await transaction.RollbackAsync(token);
                return new()
                {
                    Messages = [new()
                     {
                         TypeMessage = MessagesTypesEnum.Error,
                         Text = $"В следствии изменения документа - сумма баланса [wallet:{paymentDb.Wallet.WalletType}] станет отрицательной"
                     }]
                };
            }
            else
            {
                await context.WalletsRetail
                    .Where(x => x.Id == req.Payload.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.Payload.Amount), cancellationToken: token);

                await context.WalletsRetail
                    .Where(x => x.Id == paymentDb.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - paymentDb.Amount), cancellationToken: token);
            }
        }
        else if (req.Payload.StatusPayment != paymentDb.StatusPayment)
        {
            if (req.Payload.StatusPayment == PaymentsRetailStatusesEnum.Paid)
            {
                await context.WalletsRetail
                    .Where(x => x.Id == req.Payload.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.Payload.Amount), cancellationToken: token);
            }
            else if (paymentDb.StatusPayment == PaymentsRetailStatusesEnum.Paid)
            {
                if (paymentDb.Wallet!.Balance < req.Payload.Amount)
                {
                    await transaction.RollbackAsync(token);
                    return new()
                    {
                        Messages = [new()
                        {
                            Text = $"В следствии изменения документа - сумма баланса [wallet:{paymentDb.Wallet.WalletType}] станет отрицательной",
                            TypeMessage = MessagesTypesEnum.Error,
                        }]
                    };
                }
                await context.WalletsRetail
                    .Where(x => x.Id == paymentDb.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - req.Payload.Amount), cancellationToken: token);
            }
        }

        await context.PaymentsRetailDocuments
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.Description, req.Payload.Description)
                .SetProperty(p => p.WalletId, req.Payload.WalletId)
                .SetProperty(p => p.Version, _ng)
                .SetProperty(p => p.TypePaymentId, req.Payload.TypePaymentId)
                .SetProperty(p => p.StatusPayment, req.Payload.StatusPayment)
                .SetProperty(p => p.PaymentSource, req.Payload.PaymentSource)
                .SetProperty(p => p.DatePayment, req.Payload.DatePayment)
                .SetProperty(p => p.Amount, req.Payload.Amount)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        return new()
        {
            Response = _ng,
            Messages = [new()
            {
                Text = "Ok",
                TypeMessage = MessagesTypesEnum.Success,
            }]
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentRetailDocumentModelDB> q = context.PaymentsRetailDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

        if (!string.IsNullOrWhiteSpace(req.Payload?.PayerFilterIdentityId))
            q = q.Where(x => context.WalletsRetail.Any(y => y.Id == x.WalletId && y.UserIdentityId == req.Payload.PayerFilterIdentityId));

        if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Count != 0)
            q = q.Where(x => req.Payload.TypesFilter.Contains(x.TypePaymentId));

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Length != 0)
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusPayment));

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DatePayment >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DatePayment <= req.Payload.End);
        }

        if (req.Payload is not null && req.Payload.ExcludeOrderId > 0)
            q = q.Where(x => !context.PaymentsOrdersLinks.Any(y => y.PaymentDocumentId == x.Id && y.OrderDocumentId == req.Payload.ExcludeOrderId));

        IOrderedQueryable<PaymentRetailDocumentModelDB> oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => q.OrderBy(x => x.DatePayment),
            DirectionsEnum.Down => q.OrderByDescending(x => x.DatePayment),
            _ => q.OrderBy(x => x.Name)
        };

        IQueryable<PaymentRetailDocumentModelDB>? pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq
                .Include(x => x.Wallet!)
                .ThenInclude(x => x.WalletType)
                .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.Ids is null || req.Ids.Length == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Ids is null || Ids.Length == 0" }] };

        return new()
        {
            Response = await context.PaymentsRetailDocuments
            .Where(x => req.Ids.Contains(x.Id))
            .Include(x => x.Wallet)
            .ToArrayAsync(cancellationToken: token)
        };
    }
}