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
public partial class InstrumentsTableStockSharpComponent : StockSharpAboutComponent
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;


    InstrumentTradeStockSharpViewModel? manualOrderContext;
    bool ManualOrderCreating;
    private readonly DialogOptions _dialogOptions = new() { FullWidth = true, MaxWidth = MaxWidth.ExtraLarge };

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

    List<InstrumentTradeStockSharpViewModel> InstrumentsPartData;
    IEnumerable<BoardStockSharpViewModel>? _selectedBoards;
    IEnumerable<BoardStockSharpViewModel> SelectedBoards
    {
        get => _selectedBoards ?? [];
        set
        {
            _selectedBoards = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
        }
    }

    MudTable<InstrumentTradeStockSharpViewModel>? _tableRef;
    private string? searchString = null;

    readonly List<BoardStockSharpViewModel> Boards = [];

    string StyleSup(InstrumentTradeStockSharpViewModel ctx) => EachDisable || ctx.LastUpdatedAtUTC < AboutConnection!.LastConnectedAt ? "" : "cursor:pointer;";

    string ClassSup(InstrumentTradeStockSharpViewModel ctx) => EachDisable || ctx.LastUpdatedAtUTC < AboutConnection!.LastConnectedAt ? "ms-1 text-default bi bi-arrow-through-heart opacity-25" : "ms-1 text-primary bi bi-arrow-through-heart";

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadBoards();
    }

    bool _stateFilter;
    async Task StateFilterToggle()
    {
        _stateFilter = !_stateFilter;
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    void ManualOrder(InstrumentTradeStockSharpViewModel req)
    {
        manualOrderContext = req;
        ManualOrderCreating = true;
    }

    async Task ReloadBoards()
    {
        TResponseModel<List<BoardStockSharpViewModel>> boardsRes = await SsRepo.GetBoardsAsync();
        lock (Boards)
        {
            Boards.Clear();
            if (boardsRes.Response is not null)
                Boards.AddRange(boardsRes.Response);
        }
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
            BoardsFilter = [.. SelectedBoards.Select(x => x.Id)],
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
        InstrumentsPartData = res.Response;
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpViewModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}