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
    public async Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(TAuthRequestStandardModel<CreateWalletConversionRetailDocumentRequestModel> req, CancellationToken token = default)
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
        CreateWalletConversionRetailDocumentRequestModel createDoc = req.Payload;

        if (createDoc.ToWalletId < 1 || createDoc.ToWalletId < 1)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите кошельки списания и зачисления!"
                }]
            };

        if (createDoc.ToWalletSum <= 0 || createDoc.FromWalletSum <= 0)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите сумму списания и зачисления!"
                }]
            };

        if (createDoc.ToWalletId == createDoc.FromWalletId)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Счёт списания не может совпадать со счётом зачисления"
                }]
            };
        TResponseModel<int> res = new();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        WalletRetailModelDB[] walletsDb = await context.WalletsRetail
            .Where(x => x.Id == createDoc.FromWalletId || x.Id == createDoc.ToWalletId)
            .Include(x => x.WalletType)
            .ToArrayAsync(cancellationToken: token);

        WalletRetailModelDB
            walletSender = walletsDb.First(x => x.Id == createDoc.FromWalletId),
            walletRecipient = walletsDb.First(x => x.Id == createDoc.ToWalletId);

        if (!walletSender.WalletType!.IsSystem && !walletSender.WalletType!.IgnoreBalanceChanges && walletSender.Balance - createDoc.FromWalletSum < 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!walletSender.WalletType.IgnoreBalanceChanges)
            await context.WalletsRetail.Where(x => x.Id == createDoc.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - createDoc.FromWalletSum), cancellationToken: token);

        if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
            await context.WalletsRetail.Where(x => x.Id == createDoc.ToWalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance + createDoc.ToWalletSum), cancellationToken: token);

        createDoc.Name = createDoc.Name.Trim();
        createDoc.Version = Guid.NewGuid();
        createDoc.ToWallet = null;
        createDoc.FromWallet = null;
        createDoc.CreatedAtUTC = DateTime.UtcNow;
        createDoc.DateDocument = createDoc.DateDocument.SetKindUtc();

        WalletConversionRetailDocumentModelDB docDb = WalletConversionRetailDocumentModelDB.Build(createDoc);

        await context.ConversionsDocumentsWalletsRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.AddSuccess($"Документ перевода/конвертации создан #{docDb.Id}");

        if (createDoc.InjectToOrderId > 0)
        {
            await context.ConversionsOrdersLinksRetail.AddAsync(new()
            {
                ConversionDocumentId = docDb.Id,
                OrderDocumentId = createDoc.InjectToOrderId,
                AmountPayment = createDoc.ToWalletSum,
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь документа перевода/конвертации #{docDb.Id} с заказом #{createDoc.InjectToOrderId}");
        }

        await transaction.CommitAsync(token);
        return new TResponseModel<int>() { Response = docDb.Id };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateConversionDocumentRetailAsync(TAuthRequestStandardModel<WalletConversionRetailDocumentModelDB> req, CancellationToken token = default)
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

        if (req.Payload.ToWalletId < 1 || req.Payload.ToWalletId < 1)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите кошельки списания и зачисления!"
                }]
            };

        if (req.Payload.ToWalletSum <= 0 || req.Payload.FromWalletSum <= 0)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите сумму списания и зачисления!"
                }]
            };

        if (req.Payload.ToWalletId == req.Payload.FromWalletId)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Счёт списания не может совпадать со счётом зачисления"
                }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        WalletConversionRetailDocumentModelDB _conversionDocDb = await context.ConversionsDocumentsWalletsRetail.FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);
        int[] _walletsIds = [req.Payload.FromWalletId, req.Payload.ToWalletId, _conversionDocDb.FromWalletId, _conversionDocDb.ToWalletId];

        WalletRetailModelDB[] walletsDb = await context.WalletsRetail
           .Where(x => _walletsIds.Contains(x.Id))
           .Include(x => x.WalletType)
           .ToArrayAsync(cancellationToken: token);

        WalletRetailModelDB
            walletSenderDb = walletsDb.First(x => x.Id == _conversionDocDb.FromWalletId),
            walletRecipientDb = walletsDb.First(x => x.Id == _conversionDocDb.ToWalletId),
            walletSender = walletsDb.First(x => x.Id == req.Payload.FromWalletId),
            walletRecipient = walletsDb.First(x => x.Id == req.Payload.ToWalletId);

        if (_conversionDocDb.Version != req.Payload.Version)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Документ уже кем-то изменён. Обновите страницу с документом и повторите попытку"
                }]
            };

        decimal
            _deltaSender = req.Payload.FromWalletSum - _conversionDocDb.FromWalletSum,
            _deltaRecipient = req.Payload.ToWalletSum - _conversionDocDb.ToWalletSum;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (req.Payload.FromWalletId == _conversionDocDb.FromWalletId)
        {
            if (!walletSender.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.Payload.FromWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - _deltaSender), cancellationToken: token);
        }
        else
        {
            if (!walletSenderDb.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == _conversionDocDb.FromWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + _conversionDocDb.FromWalletSum), cancellationToken: token);

            if (!walletSender.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.Payload.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - req.Payload.FromWalletSum), cancellationToken: token);
        }

        if (req.Payload.ToWalletId == _conversionDocDb.ToWalletId)
        {
            if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.Payload.ToWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance + _deltaRecipient), cancellationToken: token);
        }
        else
        {
            if (!walletRecipientDb.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == _conversionDocDb.ToWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - _conversionDocDb.ToWalletSum), cancellationToken: token);

            if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.Payload.ToWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.Payload.ToWalletSum), cancellationToken: token);
        }
        Guid _ng = Guid.NewGuid();
        await context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name.Trim())
                .SetProperty(p => p.FromWalletId, req.Payload.FromWalletId)
                .SetProperty(p => p.FromWalletSum, req.Payload.FromWalletSum)
                .SetProperty(p => p.ToWalletId, req.Payload.ToWalletId)
                .SetProperty(p => p.ToWalletSum, req.Payload.ToWalletSum)
                .SetProperty(p => p.Version, _ng)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        return new()
        {
            Response = _ng,
            Messages = [new()
            {
                TypeMessage = MessagesTypesEnum.Success,
                Text = "Ok"
            }]
        };
    }

    /// <inheritdoc/> 
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsRetailAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default)
    {
        if (req.Ids.Length == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Ids.Length == 0" }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.ConversionsDocumentsWalletsRetail
                .Where(x => req.Ids.Contains(x.Id))
                .Include(x => x.ToWallet)
                .Include(x => x.FromWallet)
                .ToArrayAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsRetailAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail.AsQueryable();

        if (req.Payload?.IncludeDisabled != true)
            q = q.Where(x => !x.IsDisabled);

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        string[]
            sendersUserFilter = req.Payload?.SendersUserFilter ?? [],
            recipientsUserFilter = req.Payload?.RecipientsUserFilter ?? [];

        q = from doc in q
            join sender in context.WalletsRetail on doc.FromWalletId equals sender.Id
            join recipient in context.WalletsRetail on doc.ToWalletId equals recipient.Id

            where sendersUserFilter.Length == 0 || sendersUserFilter.Contains(sender.UserIdentityId) || recipientsUserFilter.Contains(recipient.UserIdentityId)

            select doc;

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        if (req.Payload is not null && req.Payload.ExcludeOrderId > 0)
            q = q.Where(x => !context.ConversionsOrdersLinksRetail.Any(y => y.ConversionDocumentId == x.Id && y.OrderDocumentId == req.Payload.ExcludeOrderId));

        IOrderedQueryable<WalletConversionRetailDocumentModelDB> oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => q.OrderBy(x => x.DateDocument),
            DirectionsEnum.Down => q.OrderByDescending(x => x.DateDocument),
            _ => q.OrderBy(x => x.Name)
        };

        IQueryable<WalletConversionRetailDocumentModelDB> pq = oq
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
                .Include(x => x.FromWallet).ThenInclude(x => x!.WalletType).Include(x => x.Orders)
                .Include(x => x.ToWallet).ThenInclude(x => x!.WalletType).Include(x => x.Orders)
                .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB>> DeleteToggleConversionRetailAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        int conversionId = req.Payload;
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == conversionId);
        TResponseModel<WalletConversionRetailDocumentModelDB> res = new()
        {
            Response = await q
            .Include(x => x.ToWallet!).ThenInclude(x => x.WalletType)
            .Include(x => x.FromWallet!).ThenInclude(x => x.WalletType)
            .FirstAsync(x => x.Id == conversionId, cancellationToken: token)
        };

        if (res.Response.ToWalletId < 1 || res.Response.ToWalletId < 1)
        {
            res.AddError("Укажите кошельки списания и зачисления!");
            return res;
        }

        if (res.Response.ToWalletSum <= 0 || res.Response.FromWalletSum <= 0)
        {
            res.AddError("Укажите сумму списания и зачисления!");
            return res;
        }

        if (res.Response.ToWalletId == res.Response.FromWalletId)
        {
            res.AddError("Счёт списания не может совпадать со счётом зачисления");
            return res;
        }

        res.Response.IsDisabled = !res.Response.IsDisabled;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!res.Response.FromWallet!.WalletType!.IsSystem && !res.Response.FromWallet.WalletType!.IgnoreBalanceChanges && res.Response.FromWallet.Balance < res.Response.FromWalletSum)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        await context.WalletsRetail
                .Where(x => x.Id == res.Response.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, b => b.Balance - res.Response.FromWalletSum), cancellationToken: token);

        if (!res.Response.ToWallet!.WalletType!.IgnoreBalanceChanges)
        {
            await context.WalletsRetail
                .Where(x => x.Id == res.Response.ToWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, b => b.Balance + res.Response.ToWalletSum), cancellationToken: token);
        }

        int rc = await q.ExecuteUpdateAsync(set => set
                     .SetProperty(p => p.IsDisabled, res.Response.IsDisabled)
                     .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);
        res.AddSuccess($"Документ: успешно {(res.Response.IsDisabled ? "выключен" : "включён")}");
        return res;
    }
}