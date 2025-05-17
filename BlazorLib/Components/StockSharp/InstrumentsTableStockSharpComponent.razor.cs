////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsTableStockSharpComponent
/// </summary>
public partial class InstrumentsTableStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;


    IEnumerable<InstrumentsStockSharpTypesEnum>? _typesSelected;
    IEnumerable<InstrumentsStockSharpTypesEnum>? TypesSelected
    {
        get => _typesSelected;
        set
        {
            _typesSelected = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
        }
    }

    IEnumerable<CurrenciesTypesEnum>? _currenciesSelected;
    IEnumerable<CurrenciesTypesEnum>? CurrenciesSelected
    {
        get => _currenciesSelected;
        set
        {
            _currenciesSelected = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
        }
    }


    MudTable<InstrumentTradeStockSharpViewModel>? _tableRef;
    private string? searchString = null;

    bool _stateFilter;
    async Task StateFilterToggle()
    {
        _stateFilter = !_stateFilter;
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    async Task FavoriteToggle(InstrumentTradeStockSharpViewModel sender)
    {
        await SetBusyAsync();
        ResponseBaseModel res = await SsRepo.InstrumentFavoriteToggleAsync(sender);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    async Task<TableData<InstrumentTradeStockSharpViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        InstrumentsRequestModel req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
            CurrenciesFilter = CurrenciesSelected is null || !CurrenciesSelected.Any() ? null : [.. CurrenciesSelected],
            TypesFilter = TypesSelected is null || !TypesSelected.Any() ? null : [.. TypesSelected],
            FavoriteFilter = _stateFilter ? true : null
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = await SsRepo.InstrumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpViewModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}