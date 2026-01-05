////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Bank;

/// <summary>
/// TBankAccountsComponent
/// </summary>
public partial class TBankAccountsComponent
{
    [Inject]
    IBankService BankRepo { get; set; } = default!;


    MudTable<TBankAccountModelDB>? table;
    string? searchString;


    async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<TBankAccountModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectAccountsRequestModel> req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert()
        };
        TPaginationResponseStandardModel<TBankAccountModelDB> res = await BankRepo.AccountsTBankSelectAsync(req, token);
        return new TableData<TBankAccountModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}