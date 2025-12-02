////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// NomenclaturesManageComponent
/// </summary>
public partial class NomenclaturesManageComponent : BlazorRegistersComponent
{
    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;


    /// <summary>
    /// ContextName
    /// </summary>
    [Parameter]
    public string? ContextName { get; set; }


    bool _expanded;
    MudTable<NomenclatureModelDB> tableRef = default!;


    async void CreateNomenclatureAction(NomenclatureModelDB nom)
    {
        await tableRef.ReloadServerData();
        OnExpandCollapseClick();
        StateHasChanged();
    }

    /// <summary>
    /// Скачать полный прайс
    /// </summary>
    async Task DownloadFullPrice()
    {
        await SetBusyAsync();
        FileAttachModel res = await CommerceRepo.PriceFullFileGetAsync();

        await SetBusyAsync(false);
        if (res.Data.Length != 0)
        {
            using MemoryStream ms = new(res.Data);
            using DotNetStreamReference streamRef = new(stream: ms);
            await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", res.Name, streamRef);
        }
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<NomenclatureModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req = new()
        {
            Payload = new() { ContextName = ContextName },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<NomenclatureModelDB> res = await CommerceRepo.NomenclaturesSelectAsync(req, token);

        if (res.Response is not null)
        {
            await CacheRegistersUpdate(offers: [], goods: res.Response.Select(x => x.Id).ToArray());
            IsBusyProgress = false;
            return new TableData<NomenclatureModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
        }

        IsBusyProgress = false;

        if (res.Response is null)
            return new TableData<NomenclatureModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<NomenclatureModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }
}