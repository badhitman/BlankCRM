////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Bank;

/// <summary>
/// TransfersBanksTableComponent
/// </summary>
public partial class TransfersBanksTableComponent
{
    [Inject]
    IBankService BankRepo { get; set; } = default!;


    MudTable<BankTransferModelDB>? table;
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
    private async Task<TableData<BankTransferModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectTransfersBanksRequestModel> req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert()
        };
        TPaginationResponseStandardModel<BankTransferModelDB> res = await BankRepo.BanksTransfersSelectAsync(req, token);
        return new TableData<BankTransferModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}