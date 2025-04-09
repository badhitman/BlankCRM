////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
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

        await SetBusyAsync(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        PartData = res.Response ?? [];
        return new TableData<KladrResponseModel>() { TotalItems = res.TotalRowsCount, Items = PartData };
    }
}