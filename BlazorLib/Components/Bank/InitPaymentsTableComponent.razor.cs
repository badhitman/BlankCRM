////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Bank;

/// <summary>
/// InitPaymentsTableComponent
/// </summary>
public partial class InitPaymentsTableComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IMerchantService MerchRepo { get; set; } = default!;


    MudTable<PaymentInitTBankResultModelDB>? table;
    string? searchString = null;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    async Task<TableData<PaymentInitTBankResultModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectInitPaymentsTBankRequestModel> req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        TPaginationResponseStandardModel<PaymentInitTBankResultModelDB> res = await MerchRepo.PaymentsInitSelectTBankAsync(req, token);
        return new TableData<PaymentInitTBankResultModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}