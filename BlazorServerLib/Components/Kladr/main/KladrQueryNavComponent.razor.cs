////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Kladr.main;

/// <summary>
/// KladrQueryNavComponent
/// </summary>
public partial class KladrQueryNavComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public string? CodeLikeFilter { get; set; }


    private MudTable<KladrResponseModel>? table;
    private string? searchString = null;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        KladrSelectRequestModel req = new()
        {
            FindQuery = searchString ?? "",
            CodeLikeFilter = CodeLikeFilter,
            PageNum = state.Page,
            PageSize = state.PageSize,
        };

        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsSelect(req);
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