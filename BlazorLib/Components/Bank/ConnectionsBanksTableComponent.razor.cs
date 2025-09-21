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
public partial class ConnectionsBanksTableComponent
{
    [Inject]
    IBankService BankRepo { get; set; } = default!;


    MudTable<BankConnectionModelDB>? table;
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
        TPaginationResponseModel<BankConnectionModelDB> res = await BankRepo.ConnectionsBanksSelectAsync(req, token);
        return new TableData<BankConnectionModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}