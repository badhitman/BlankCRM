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
    public async Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(CreateWalletConversionRetailDocumentRequestModel req, CancellationToken token = default)
    {
        if (req.ToWalletId < 1 || req.ToWalletId < 1)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите кошельки списания и зачисления!"
                }]
            };

        if (req.ToWalletSum <= 0 || req.FromWalletSum <= 0)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите сумму списания и зачисления!"
                }]
            };

        if (req.ToWalletId == req.FromWalletId)
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
            .Where(x => x.Id == req.FromWalletId || x.Id == req.ToWalletId)
            .Include(x => x.WalletType)
            .ToArrayAsync(cancellationToken: token);

        WalletRetailModelDB
            walletSender = walletsDb.First(x => x.Id == req.FromWalletId),
            walletRecipient = walletsDb.First(x => x.Id == req.ToWalletId);

        if (!walletSender.WalletType!.IsSystem && !walletSender.WalletType!.IgnoreBalanceChanges && walletSender.Balance - req.FromWalletSum < 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!walletSender.WalletType.IgnoreBalanceChanges)
            await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - req.FromWalletSum), cancellationToken: token);

        if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
            await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance + req.ToWalletSum), cancellationToken: token);

        req.Name = req.Name.Trim();
        req.Version = Guid.NewGuid();
        req.ToWallet = null;
        req.FromWallet = null;
        req.CreatedAtUTC = DateTime.UtcNow;
        req.DateDocument = req.DateDocument.SetKindUtc();

        WalletConversionRetailDocumentModelDB docDb = WalletConversionRetailDocumentModelDB.Build(req);

        await context.ConversionsDocumentsWalletsRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.AddSuccess($"Документ перевода/конвертации создан #{docDb.Id}");

        if (req.InjectToOrderId > 0)
        {
            await context.ConversionsOrdersLinksRetail.AddAsync(new()
            {
                ConversionDocumentId = docDb.Id,
                OrderDocumentId = req.InjectToOrderId,
                AmountPayment = req.ToWalletSum,
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь документа перевода/конвертации #{docDb.Id} с заказом #{req.InjectToOrderId}");
        }

        await transaction.CommitAsync(token);
        return new TResponseModel<int>() { Response = docDb.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionDocumentRetailAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
    {
        if (req.ToWalletId < 1 || req.ToWalletId < 1)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите кошельки списания и зачисления!"
                }]
            };

        if (req.ToWalletSum <= 0 || req.FromWalletSum <= 0)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите сумму списания и зачисления!"
                }]
            };

        if (req.ToWalletId == req.FromWalletId)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Счёт списания не может совпадать со счётом зачисления"
                }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        WalletConversionRetailDocumentModelDB _conversionDocDb = await context.ConversionsDocumentsWalletsRetail.FirstAsync(x => x.Id == req.Id, cancellationToken: token);
        int[] _walletsIds = [req.FromWalletId, req.ToWalletId, _conversionDocDb.FromWalletId, _conversionDocDb.ToWalletId];

        WalletRetailModelDB[] walletsDb = await context.WalletsRetail
           .Where(x => _walletsIds.Contains(x.Id))
           .Include(x => x.WalletType)
           .ToArrayAsync(cancellationToken: token);

        WalletRetailModelDB
            walletSenderDb = walletsDb.First(x => x.Id == _conversionDocDb.FromWalletId),
            walletRecipientDb = walletsDb.First(x => x.Id == _conversionDocDb.ToWalletId),
            walletSender = walletsDb.First(x => x.Id == req.FromWalletId),
            walletRecipient = walletsDb.First(x => x.Id == req.ToWalletId);

        if (_conversionDocDb.Version != req.Version)
            return ResponseBaseModel.CreateError("Документ уже кем-то изменён. Обновите страницу с документом и повторите попытку");

        decimal
            _deltaSender = req.FromWalletSum - _conversionDocDb.FromWalletSum,
            _deltaRecipient = req.ToWalletSum - _conversionDocDb.ToWalletSum;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (req.FromWalletId == _conversionDocDb.FromWalletId)
        {
            if (!walletSender.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
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
                await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - req.FromWalletSum), cancellationToken: token);
        }

        if (req.ToWalletId == _conversionDocDb.ToWalletId)
        {
            if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
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
                await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.ToWalletSum), cancellationToken: token);
        }

        await context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name.Trim())
                .SetProperty(p => p.FromWalletId, req.FromWalletId)
                .SetProperty(p => p.FromWalletSum, req.FromWalletSum)
                .SetProperty(p => p.ToWalletId, req.ToWalletId)
                .SetProperty(p => p.ToWalletSum, req.ToWalletSum)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
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
    public async Task<ResponseBaseModel> DeleteToggleConversionRetailAsync(int conversionId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == conversionId);

        WalletConversionRetailDocumentModelDB conversionDb = await q
            .Include(x => x.ToWallet!).ThenInclude(x => x.WalletType)
            .Include(x => x.FromWallet!).ThenInclude(x => x.WalletType)
            .FirstAsync(x => x.Id == conversionId, cancellationToken: token);

        if (conversionDb.ToWalletId < 1 || conversionDb.ToWalletId < 1)
            return ResponseBaseModel.CreateError("Укажите кошельки списания и зачисления!");

        if (conversionDb.ToWalletSum <= 0 || conversionDb.FromWalletSum <= 0)
            return ResponseBaseModel.CreateError("Укажите сумму списания и зачисления!");

        if (conversionDb.ToWalletId == conversionDb.FromWalletId)
            return ResponseBaseModel.CreateError("Счёт списания не может совпадать со счётом зачисления");

        conversionDb.IsDisabled = !conversionDb.IsDisabled;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!conversionDb.FromWallet!.WalletType!.IsSystem && !conversionDb.FromWallet.WalletType!.IgnoreBalanceChanges && conversionDb.FromWallet.Balance < conversionDb.FromWalletSum)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        await context.WalletsRetail
                .Where(x => x.Id == conversionDb.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, b => b.Balance - conversionDb.FromWalletSum), cancellationToken: token);

        if (!conversionDb.ToWallet!.WalletType!.IgnoreBalanceChanges)
        {
            await context.WalletsRetail
                .Where(x => x.Id == conversionDb.ToWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, b => b.Balance + conversionDb.ToWalletSum), cancellationToken: token);
        }

        int res = await q.ExecuteUpdateAsync(set => set
                     .SetProperty(p => p.IsDisabled, conversionDb.IsDisabled)
                     .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);

        return
            ResponseBaseModel
            .CreateSuccess($"Документ: успешно {(conversionDb.IsDisabled ? "выключен" : "включён")}");
    }
}