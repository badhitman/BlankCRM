////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Kladr.control;

/// <summary>
/// KladrFindTableResultComponent
/// </summary>
public partial class KladrFindTableResultComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IKladrNavigationService kladrRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? FindText { get; set; }


    List<KladrResponseModel>? PartData;
    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<KladrResponseModel>> ServerReload(TableState state, CancellationToken token)
    {
        KladrFindRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            FindText = $"%{FindText}%",
        };

        await SetBusy(token: token);
        TPaginationResponseModel<KladrResponseModel> res = await kladrRepo.ObjectsFind(req);
        await SetBusy(false, token: token);
        PartData = res.Response ?? [];
        return new TableData<KladrResponseModel>()
        {
            TotalItems = res.TotalRowsCount,
            Items = PartData
        };
    }
}