////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Bank;

/// <summary>
/// CustomersBanksTableComponent
/// </summary>
public partial class CustomersBanksTableComponent
{
    [Inject]
    IBankService BankRepo { get; set; } = default!;


    MudTable<CustomerBankIdModelDB>? table;
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
    private async Task<TableData<CustomerBankIdModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel> req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert()
        };
        TPaginationResponseModel<CustomerBankIdModelDB> res = await BankRepo.CustomersBanksSelectAsync(req, token);
        return new TableData<CustomerBankIdModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}