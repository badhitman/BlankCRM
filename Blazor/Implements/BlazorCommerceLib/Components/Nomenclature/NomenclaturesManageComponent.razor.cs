////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorCommerceLib.Components.Nomenclature;

/// <summary>
/// NomenclaturesManageComponent
/// </summary>
public partial class NomenclaturesManageComponent : BlazorRegistersComponent
{
    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;


    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;


    /// <summary>
    /// ContextName
    /// </summary>
    [Parameter]
    public string? ContextName { get; set; }


    bool? enabledOnlyFilter;
    bool? EnabledOnlyFilter
    {
        get => enabledOnlyFilter;
        set
        {
            enabledOnlyFilter = value;
            InvokeAsync(tableRef.ReloadServerData);
            InvokeAsync(SaveFilters);
        }
    }

    bool _expanded;
    MudTable<NomenclatureModelDB> tableRef = default!;


    async Task SaveFilters()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await StorageRepo.SaveParameterAsync(EnabledOnlyFilter, GlobalStaticCloudStorageMetadata.RetailNomenclaturesEnabledOnlyFilter(CurrentUserSession.UserId), true, false);
    }

    async void CreateNomenclatureAction(NomenclatureModelDB nom)
    {
        await tableRef.ReloadServerData();
        OnExpandCollapseClick();
        StateHasChanged();
    }

    /// <summary>
    /// Скачать полный прайс (Excel)
    /// </summary>
    async Task DownloadFullPriceExcel()
    {
        await SetBusyAsync();
        FileAttachModel res = await CommerceRepo.PriceFullFileGetExcelAsync();

        await SetBusyAsync(false);
        if (res.Data.Length != 0)
        {
            using MemoryStream ms = new(res.Data);
            using DotNetStreamReference streamRef = new(stream: ms);
            await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", res.Name, streamRef);
        }
    }

    /// <summary>
    /// Скачать полный прайс (JSON)
    /// </summary>
    async Task DownloadFullPriceJSON()
    {
        await SetBusyAsync();
        FileAttachModel res = await CommerceRepo.PriceFullFileGetJsonAsync();

        await SetBusyAsync(false);
        if (res.Data.Length != 0)
        {
            using MemoryStream ms = new(res.Data);
            using DotNetStreamReference streamRef = new(stream: ms);
            await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", res.Name, streamRef);
        }
    }

    /// <inheritdoc/>
    private async Task<TableData<NomenclatureModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<NomenclaturesSelectRequestModel> req = new()
        {
            Payload = new()
            {
                ContextName = ContextName,
                EnabledOnly = EnabledOnlyFilter
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<NomenclatureModelDB> res = await CommerceRepo.NomenclaturesSelectAsync(req, token);

        if (res.Response is not null)
        {
            await CacheRegistersUpdate(_offers: [], _goods: [.. res.Response.Select(x => x.Id)]);
        }

        await SetBusyAsync(false, token);

        if (res.Response is null)
            return new TableData<NomenclatureModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<NomenclatureModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
        TResponseModel<bool?> _readEnabledOnlyFilter = await StorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.RetailNomenclaturesEnabledOnlyFilter(CurrentUserSession.UserId));
        if (_readEnabledOnlyFilter.Success() && _readEnabledOnlyFilter.Response is not null)
        {
            enabledOnlyFilter = _readEnabledOnlyFilter.Response;
            if (tableRef is not null)
                await tableRef.ReloadServerData();
        }
    }
}