////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Kladr.main;

/// <summary>
/// QueryNavKladrComponent
/// </summary>
public partial class QueryNavKladrComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? CodeLikeFilter { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required QueryNavKladrComponent? Parent { get; set; }

    /// <inheritdoc/>
    public List<KladrResponseModel> PartData = [];

    private MudTable<KladrResponseModel>? table;


    /// <inheritdoc/>
    public void Reload()
    {
        StateHasChangedCall();
        //if (table is not null)
        //    await table.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        KladrSelectRequestModel req = new()
        {
            CodeLikeFilter = CodeLikeFilter,
            PageNum = state.Page,
            PageSize = state.PageSize,
        };

        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsSelect(req);
        await SetBusy(false, token: token);
        PartData = res.Response ?? [];
        return new TableData<KladrResponseModel>() { TotalItems = res.TotalRowsCount, Items = PartData };
    }
}