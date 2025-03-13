////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Kladr;

/// <summary>
/// KladrQueryNavComponent
/// </summary>
public partial class KladrQueryNavComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;


    private MudTable<KladrResponseModel>? table;
    private string? searchString = null;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(searchString) || searchString.Trim().Length < 3)
            return new TableData<KladrResponseModel>() { TotalItems = 0, Items = [] };
        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsSelect(new KladrSelectRequestModel() { FindQuery = searchString, });
        await SetBusy(false, token: token);
        return new TableData<KladrResponseModel>() { TotalItems = res.TotalRowsCount, Items = res.Response ?? [] };
    }

    private async Task OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}