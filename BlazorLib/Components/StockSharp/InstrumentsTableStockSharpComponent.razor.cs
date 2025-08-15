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

    [Inject]
    IDialogService DialogRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;


    InstrumentTradeStockSharpViewModel? manualOrderContext;
    bool ManualOrderCreating;
    private readonly DialogOptions _dialogOptions = new() { FullWidth = true, MaxWidth = MaxWidth.ExtraLarge };

    static readonly StorageMetadataModel setBoards = new()
    {
        ApplicationName = nameof(InstrumentsTableStockSharpComponent),
        PrefixPropertyName = "settings",
        PropertyName = $"form-set.{nameof(SelectedBoards)}"
    };

    static readonly StorageMetadataModel setCol = new()
    {
        ApplicationName = nameof(InstrumentsTableStockSharpComponent),
        PrefixPropertyName = "settings",
        PropertyName = $"form-set.{nameof(ColumnsSelected)}"
    };

    static readonly StorageMetadataModel filterMarkers = new()
    {
        ApplicationName = nameof(InstrumentsTableStockSharpComponent),
        PrefixPropertyName = "settings",
        PropertyName = $"form-set.{nameof(MarkersSelected)}"
    };

    static string _mtp = nameof(InstrumentTradeStockSharpModel.Multiplier),
        _std = nameof(InstrumentTradeStockSharpModel.SettlementDate),
        _fv = nameof(InstrumentTradeStockSharpModel.FaceValue),
        _dc = nameof(InstrumentTradeStockSharpModel.Decimals),
        _ps = nameof(InstrumentTradeStockSharpModel.PriceStep),
        _isin = nameof(InstrumentTradeStockSharpViewModel.ISIN),
        _issD = nameof(InstrumentTradeStockSharpViewModel.IssueDate),
        _mtD = nameof(InstrumentTradeStockSharpViewModel.MaturityDate),
        _cr = nameof(InstrumentTradeStockSharpViewModel.CouponRate),
        _lfP = nameof(InstrumentTradeStockSharpViewModel.LastFairPrice),
        _cmnt = nameof(InstrumentTradeStockSharpViewModel.Comment),
        _mcs = "Markers", _rbcs = "Rubrics";
    static string[] columnsExt = [_mtp, _dc, _std, _fv, _mcs, _isin, _ps, _issD, _mtD, _cr, _lfP, _cmnt, _rbcs];


    IEnumerable<string>? _columnsSelected;
    IEnumerable<string>? ColumnsSelected
    {
        get => _columnsSelected;
        set
        {
            _columnsSelected = value;
            InvokeAsync(SaveParameters);
        }
    }

    IEnumerable<MarkersInstrumentStockSharpEnum?> _markersSelected = [];
    IEnumerable<MarkersInstrumentStockSharpEnum?> MarkersSelected
    {
        get => _markersSelected;
        set
        {
            _markersSelected = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
        }
    }

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

    IEnumerable<BoardStockSharpViewModel>? _selectedBoards;
    IEnumerable<BoardStockSharpViewModel> SelectedBoards
    {
        get => _selectedBoards ?? [];
        set
        {
            _selectedBoards = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
            InvokeAsync(SaveParameters);
        }
    }

    MudTable<InstrumentTradeStockSharpViewModel>? _tableRef;
    private string? searchString = null;

    readonly List<BoardStockSharpViewModel> Boards = [];


    private static string GetMultiSelectionText(List<string?> selectedValues)
    {
        return $"{string.Join(", ", selectedValues.Select(x => x is null ? "~not set~" : x))}";//
    }


    string StyleTradeSup(InstrumentTradeStockSharpViewModel ctx) => EachDisable || ctx.LastUpdatedAtUTC < AboutConnection!.LastConnectedAt ? "" : "cursor:pointer;";

    string ClassTradeSup(InstrumentTradeStockSharpViewModel ctx)
    {
        string _res = "ms-1 bi bi-coin text-";
        return EachDisable || ctx.LastUpdatedAtUTC < AboutConnection!.LastConnectedAt
            ? $"{_res}default opacity-25"
            : $"{_res}primary";
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();

        await Task.WhenAll([
            Task.Run(async () => {
                TResponseModel<string[]> readColumnsSet = await StorageRepo.ReadParameterAsync<string[]>(setCol);
                _columnsSelected = readColumnsSet.Response;
            }),
            Task.Run(async () => {
                TResponseModel<MarkersInstrumentStockSharpEnum?[]?> markersSet = await StorageRepo.ReadParameterAsync<MarkersInstrumentStockSharpEnum?[]?>(filterMarkers);
                _markersSelected = markersSet.Response ?? [];
            }),
            Task.Run(async () => {
                TResponseModel<BoardStockSharpViewModel[]?> boardsSet = await StorageRepo.ReadParameterAsync<BoardStockSharpViewModel[]?>(setBoards);
                _selectedBoards = boardsSet.Response;
            }),
            Task.Run(ReloadBoards)]);

        await SetBusyAsync(false);
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

    async Task<IDialogReference> OpenDialogAsync(InstrumentTradeStockSharpViewModel Instrument)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.ExtraLarge, CloseOnEscapeKey = true, BackdropClick = true, FullWidth = true, };
        DialogParameters<InstrumentEditComponent> parameters = new() { { x => x.Instrument, Instrument } };
        IDialogReference res = await DialogRepo.ShowAsync<InstrumentEditComponent>($"Instrument edit: {Instrument.IdRemote}", parameters, options);
        await res.Result.WaitAsync(cancellationToken: CancellationToken.None);
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
        return res;
    }

    async Task SaveParameters()
    {
        List<Task> _tasks = [];
        if (ColumnsSelected is not null)
            _tasks.Add(Task.Run(async () => await StorageRepo.SaveParameterAsync(ColumnsSelected, setCol, true)));
        else
            _tasks.Add(Task.Run(async () => await StorageRepo.DeleteParameterAsync(setCol, true)));

        if (MarkersSelected is not null)
            _tasks.Add(Task.Run(async () => await StorageRepo.SaveParameterAsync(MarkersSelected, filterMarkers, true)));
        else
            _tasks.Add(Task.Run(async () => await StorageRepo.DeleteParameterAsync(filterMarkers, true)));

        if (SelectedBoards is not null)
            _tasks.Add(Task.Run(async () => await StorageRepo.SaveParameterAsync(SelectedBoards, setBoards, true)));
        else
            _tasks.Add(Task.Run(async () => await StorageRepo.DeleteParameterAsync(setBoards, true)));

        await Task.WhenAll(_tasks);
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
            MarkersFilter = MarkersSelected is null ? null : [.. MarkersSelected]
        };
        
        await SetBusyAsync(token: token);
        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = await SsRepo.InstrumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpViewModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}