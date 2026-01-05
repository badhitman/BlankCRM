////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Bank;

/// <summary>
/// ConnectionsBanksTableComponent
/// </summary>
public partial class ConnectionsBanksTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IBankService BankRepo { get; set; } = default!;


    MudTable<BankConnectionModelDB>? table;
    string? searchString;

    BankInterfacesEnum bankInt;
    string? bankName;
    string? bankToken;
    BankConnectionModelDB? elementBeforeEdit;

    async Task LoadAccounts(BankConnectionModelDB sender)
    {
        GetTBankAccountsRequestModel req = new() { BankConnectionId = sender.Id };
        await SetBusyAsync();
        TResponseModel<List<TBankAccountModelDB>> res = await BankRepo.GetTBankAccountsAsync(req);
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is null)
            SnackBarRepo.Error("Response is null");
        else if (res.Response.Count == 0)
            SnackBarRepo.Info("Response.Count == 0");
    }

    void BackupItem(object element)
    {
        if (element is BankConnectionModelDB _re)
            elementBeforeEdit = GlobalTools.CreateDeepCopy(_re);
    }

    async void ItemHasBeenCommitted(object element)
    {
        if (element is BankConnectionModelDB _re)
        {
            await SetBusyAsync();

            TResponseModel<int> res = await BankRepo.BankConnectionCreateOrUpdateAsync(_re);
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (table is not null)
                await table.ReloadServerData();

            elementBeforeEdit = null;
            await SetBusyAsync(false);
        }
    }

    void ResetItemToOriginalValues(object element)
    {
        ((BankConnectionModelDB)element).Token = elementBeforeEdit!.Token;
        ((BankConnectionModelDB)element).Name = elementBeforeEdit.Name;
        ((BankConnectionModelDB)element).BankInterface = elementBeforeEdit.BankInterface;
    }

    async Task AddConnection()
    {
        if (string.IsNullOrWhiteSpace(bankName) || string.IsNullOrWhiteSpace(bankToken))
        {
            SnackBarRepo.Error("set name and token for create connection");
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await BankRepo.BankConnectionCreateOrUpdateAsync(new BankConnectionModelDB() { Name = bankName, BankInterface = bankInt, Token = bankToken });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (table is not null)
            await table.ReloadServerData();

        bankName = null;
        bankToken = null;

        await SetBusyAsync(false);
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<BankConnectionModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectConnectionsBanksRequestModel> req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert()
        };
        TPaginationResponseStandardModel<BankConnectionModelDB> res = await BankRepo.ConnectionsBanksSelectAsync(req, token);
        return new TableData<BankConnectionModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}