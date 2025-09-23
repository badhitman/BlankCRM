////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using Microsoft.EntityFrameworkCore;
using SharedLib;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Json;

namespace BankService;

/// <summary>
/// BankService
/// </summary>
public partial class BankImplementService(IDbContextFactory<BankContext> bankDbFactory, IIdentityTransmission identityRepo) : IBankService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankConnectionCreateOrUpdateAsync(BankConnectionModelDB bank, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(bank);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        if (bank.Id == 0)
        {
            await ctx.ConnectionsBanks.AddAsync(bank, token);
            await ctx.SaveChangesAsync(token);
            res.Response = bank.Id;
            return res;
        }
        res.Response = bank.Id;

        await ctx.ConnectionsBanks
            .Where(x => x.Id == bank.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Token, bank.Token)
                .SetProperty(p => p.BankInterface, bank.BankInterface)
                .SetProperty(p => p.Name, bank.Name), cancellationToken: token);

        return res;
    }
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankConnectionModelDB>> ConnectionsBanksSelectAsync(TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IQueryable<BankConnectionModelDB> q = ctx.ConnectionsBanks.AsQueryable();

        if (req.Payload is not null && req.Payload.FilterOfEnabled.HasValue)
            q = q.Where(x => ctx.AccountsTBank.Any(y => y.BankConnectionId == x.Id && y.IsActive == req.Payload.FilterOfEnabled.Value));

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => (x.Token != null && x.Token.Contains(req.FindQuery)) || x.Name.Contains(req.FindQuery));

        if (req.PageSize < 10)
            req.PageSize = 10;

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageSize * req.PageNum).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> AccountTBankCreateOrUpdateAsync(TBankAccountModelDB acc, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(acc);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        if (acc.Id == 0)
        {
            await ctx.AccountsTBank.AddAsync(acc, token);
            await ctx.SaveChangesAsync(token);
            res.Response = acc.Id;
            return res;
        }
        res.Response = acc.Id;

        await ctx.AccountsTBank
            .Where(x => x.Id == acc.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsActive, acc.IsActive)
                .SetProperty(p => p.Status, acc.Status)
                .SetProperty(p => p.AccountType, acc.AccountType)
                .SetProperty(p => p.AccountNumber, acc.AccountNumber)
                .SetProperty(p => p.TariffName, acc.TariffName)
                .SetProperty(p => p.TariffCode, acc.TariffCode)
                .SetProperty(p => p.MainFlag, acc.MainFlag)
                .SetProperty(p => p.Currency, acc.Currency)
                .SetProperty(p => p.CreatedOn, acc.CreatedOn)
                .SetProperty(p => p.BankBik, acc.BankBik)
                .SetProperty(p => p.Balance, acc.Balance)
                .SetProperty(p => p.ActivationDate, acc.ActivationDate)
                .SetProperty(p => p.Name, acc.Name), cancellationToken: token);

        return res;
    }
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TBankAccountModelDB>> AccountsTBankSelectAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel> req, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IQueryable<TBankAccountModelDB> q = ctx.AccountsTBank.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.AccountNumber.Contains(req.FindQuery) || x.Name.Contains(req.FindQuery));

        if (req.PageSize < 10)
            req.PageSize = 10;

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageSize * req.PageNum).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CustomerBankCreateOrUpdateAsync(CustomerBankIdModelDB cust, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(cust);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }
        UserInfoModel? _userCustomer = null;
        BankContext ctx = default!;
        await Task.WhenAll([
            Task.Run(async () => { ctx = await bankDbFactory.CreateDbContextAsync(token); }, token),
            Task.Run(async () => { TResponseModel<UserInfoModel[]> userGet = await identityRepo.GetUsersIdentityAsync([cust.UserIdentityId], token); _userCustomer = userGet.Response?.FirstOrDefault(); }, token)
        ]);

        if (_userCustomer is null)
        {
            res.AddError($"User `{cust.UserIdentityId}` not found");
            return res;
        }

        if (cust.Id == 0)
        {
            await ctx.CustomersBanksIds.AddAsync(cust, token);
            await ctx.SaveChangesAsync(token);
            res.Response = cust.Id;
            return res;
        }
        res.Response = cust.Id;

        await ctx.CustomersBanksIds
            .Where(x => x.Id == cust.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Inn, cust.Inn)
                .SetProperty(p => p.UserIdentityId, cust.UserIdentityId)
                .SetProperty(p => p.Name, cust.Name), cancellationToken: token);

        return res;
    }
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<CustomerBankIdModelDB>> CustomersBanksSelectAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IQueryable<CustomerBankIdModelDB> q = ctx.CustomersBanksIds.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => (x.Inn != null && x.Inn.Contains(req.FindQuery)) || (x.Name != null && x.Name.Contains(req.FindQuery)));

        if (req.PageSize < 10)
            req.PageSize = 10;

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageSize * req.PageNum).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<int>> BankTransferCreateOrUpdateAsync(BankTransferModelDB trans, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        TResponseModel<int> res = new();
        ValidateReportModel ck = GlobalTools.ValidateObject(trans);
        if (!ck.IsValid)
        {
            res.Messages.InjectException(ck.ValidationResults);
            return res;
        }

        if (trans.Id == 0)
        {
            await ctx.TransfersBanks.AddAsync(trans, token);
            await ctx.SaveChangesAsync(token);
            res.Response = trans.Id;
            return res;
        }
        res.Response = trans.Id;

        await ctx.TransfersBanks
            .Where(x => x.Id == trans.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Receiver, trans.Receiver)
                .SetProperty(p => p.Amount, trans.Amount)
                .SetProperty(p => p.Sender, trans.Sender), cancellationToken: token);

        return res;
    }
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankTransferModelDB>> BanksTransfersSelectAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req, CancellationToken token = default)
    {
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);
        IQueryable<BankTransferModelDB> q = ctx.TransfersBanks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.TransactionId.Contains(req.FindQuery) || x.Sender.Contains(req.FindQuery) || x.Receiver.Contains(req.FindQuery));

        if (req.PageSize < 10)
            req.PageSize = 10;

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageSize * req.PageNum).Take(req.PageSize).ToListAsync(cancellationToken: token)
        };
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<List<TBankAccountModelDB>>> GetTBankAccountsAsync(GetTBankAccountsRequestModel req, CancellationToken token = default)
    {
        TResponseModel<List<TBankAccountModelDB>> res = new();
        BankContext ctx = await bankDbFactory.CreateDbContextAsync(token);

        BankConnectionModelDB? bankConnectionDb = ctx.ConnectionsBanks.FirstOrDefault(x => x.Id == req.BankConnectionId);
        if (bankConnectionDb is null)
        {
            res.AddError("Not found any bank connection.");
            return res;
        }

        var conMd = bankConnectionDb.BankInterface.ConnectionMetadata();
        if (conMd is null || string.IsNullOrWhiteSpace(conMd.Value.AccListRequest) || string.IsNullOrWhiteSpace(conMd.Value.BaseUrl))
        {
            res.AddError("Not found any bank connection metadata.");
            return res;
        }

        #region download operations
        TResponseModel<List<TBankAccountModel>> accountsData = await GetData<List<TBankAccountModel>>(conMd.Value.BaseUrl, conMd.Value.AccListRequest, bankConnectionDb.Token);
        if (!accountsData.Success() || accountsData.Response is null)
        {
            res.AddRangeMessages(accountsData.Messages);
            return res;
        }
        #endregion

        string[] accs = [.. accountsData.Response.Select(x => x.AccountNumber)];
        TBankAccountModelDB[] accsDb = [.. ctx.AccountsTBank.Where(x => x.BankConnectionId == req.BankConnectionId && accs.Contains(x.AccountNumber))];

        TBankAccountModel[] newAcc = [.. accountsData.Response.Where(x => !accsDb.Any(y => y.AccountNumber == x.AccountNumber))];

        if (newAcc.Length != 0)
        {
            await ctx.AccountsTBank.AddRangeAsync(newAcc.Select(x => TBankAccountModelDB.Build(x, bankConnectionDb.Id)), token);
            await ctx.SaveChangesAsync(token);
        }

        res.Response = await ctx.AccountsTBank.Where(x => x.BankConnectionId == req.BankConnectionId).ToListAsync(cancellationToken: token);
        return res;
    }
    async Task<TResponseModel<T>> GetData<T>(string baseUrl, string getRequest, string? token, Dictionary<string, object>? queryParams = null)
    {
        string url = $"{baseUrl}{getRequest}";

        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (queryParams is not null && queryParams.Count != 0)
        {
            foreach (KeyValuePair<string, object> p in queryParams)
                queryString.Add(p.Key, p.Value.ToString());

            url += $"?{queryString}";
        }

        TResponseModel<T> res = new();
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        try
        {
            res.Response = await client.GetFromJsonAsync<T>(url);
        }
        catch (Exception ex)
        {
            res.Messages.InjectException(ex);
        }
        return res;
    }
}