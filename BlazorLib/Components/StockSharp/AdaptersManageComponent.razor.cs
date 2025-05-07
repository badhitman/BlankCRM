////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// AdaptersManageComponent
/// </summary>
public partial class AdaptersManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IManageStockSharpService SsRepo { get; set; } = default!;


    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<FixMessageAdapterModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<AdaptersRequestModel> req = new()
        {
            PageSize = state.PageSize,
            PageNum = state.Page,
        };
        TPaginationResponseModel<FixMessageAdapterModelDB> res = await SsRepo.AdaptersSelectAsync(req, token);

        // Return the data
        return new TableData<FixMessageAdapterModelDB>() { TotalItems = 0, Items = [] };
    }
}