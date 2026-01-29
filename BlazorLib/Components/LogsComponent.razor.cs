////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// LogsComponent
/// </summary>
public partial class LogsComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ILogsService LogsRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;


    /// <summary>
    /// HidePanels
    /// </summary>
    [Parameter]
    public bool HidePanels { get; set; }

    /// <summary>
    /// HideAppColumn
    /// </summary>
    [Parameter]
    public bool HideAppColumn { get; set; }

    /// <summary>
    /// ApplicationsFilterSet
    /// </summary>
    [Parameter]
    public string[]? ApplicationsFilterSet { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SupplyParameterFromQuery(Name = "id")]
    public string? RowId { get; set; }


    LogsMetadataResponseModel? _metaData;
    MudDateRangePicker _picker = default!;

    DateRange _dateRangeBind = new(DateTime.Now.AddDays(-1).Date, DateTime.Now.Date);
    DateRange DateRangeBind
    {
        get => _dateRangeBind;
        set
        {
            _dateRangeBind = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    #region columns visible
    bool _AllEventProperties;
    bool AllEventProperties
    {
        get => _AllEventProperties;
        set
        {
            _AllEventProperties = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    bool _ExceptionMessage;
    bool Exception
    {
        get => _ExceptionMessage;
        set
        {
            _ExceptionMessage = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    bool _Logger;
    bool Logger
    {
        get => _Logger;
        set
        {
            _Logger = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    bool _CallSite;
    bool CallSite
    {
        get => _CallSite;
        set
        {
            _CallSite = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    bool _StackTrace;
    bool StackTrace
    {
        get => _StackTrace;
        set
        {
            _StackTrace = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    bool _ContextPrefix;
    bool ContextPrefix
    {
        get => _ContextPrefix;
        set
        {
            _ContextPrefix = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }
    #endregion

    MudTable<NLogRecordModelDB>? table;
    MudPagination? paginationRef;
    readonly HashSet<NLogRecordModelDB> favoritesRecords = [];
    List<NLogRecordModelDB>? tableItems;
    FiltersUniversalComponent? ContextsPrefixesAvailable = default!;
    FiltersUniversalComponent? ApplicationsAvailable = default!;
    FiltersUniversalComponent? LevelsAvailable = default!;
    FiltersUniversalComponent? LoggersAvailable = default!;

    int selectedRecord;
    string? navigatedRowId;
    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (table is not null && /*paginationRef is not null &&*/ !string.IsNullOrWhiteSpace(RowId) && table.FilteredItems.Any() && RowId != navigatedRowId)
        {
            navigatedRowId = RowId;
            if (int.TryParse(RowId, out selectedRecord))
                await GoToRowId(selectedRecord);
        }
        if (tableItems is not null && selectedRecord > 0)
        {
            NLogRecordModelDB? currentItem = tableItems.FirstOrDefault(x => x.Id == selectedRecord);
            if (table is not null && currentItem is not null)
            {
                await table.ScrollToItemAsync(currentItem);
                selectedRecord = 0;
            }
        }
    }

    async Task GoToRowId(int rowId)
    {
        if (table is null)
            return;
        await SetBusyAsync();
        TPaginationResponseStandardModel<NLogRecordModelDB> res = await LogsRepo.GoToPageForRowLogsAsync(new()
        {
            PageNum = table.CurrentPage,
            PageSize = table.RowsPerPage,
            Payload = new()
            {
                RowId = rowId,
                WithOutRowsData = true,
            }
        });
        //await paginationRef.NavigateToAsync(res.PageNum);
        SnackBarRepo.Info($"go to page [{res.PageNum + 1}] for id #`{rowId}`");
        await SetBusyAsync(false);
        if (table.CurrentPage != res.PageNum)
        {
            table.NavigateTo(res.PageNum);
            StateHasChanged();
        }
        else
            StateHasChanged();
    }

    void CheckedChangedAction()
    {
        if (table is not null)
            InvokeAsync(table.ReloadServerData);
    }

    string GetCheckBoxIcon(NLogRecordModelDB _row)
    {
        if (favoritesRecords.Any(x => x.Id == _row.Id))
            return Icons.Material.Filled.CheckBox;

        return Icons.Material.Filled.CheckBoxOutlineBlank;
    }

    async Task OnChipClick(NLogRecordModelDB chip)
    {
        selectedRecord = chip.Id;
        _dateRangeBind = new(null, null);

        ApplicationsAvailable = null;
        ContextsPrefixesAvailable = null;
        LevelsAvailable = null;
        LoggersAvailable = null;

        await GoToRowId(chip.Id);
    }

    /// <inheritdoc/>
    protected async Task ClipboardCopyHandle(int logRowId)
    {
        string _link = $"{NavRepo.BaseUri}/monitoring-tools?id={logRowId}&tab={GetType().Name}".Replace("//", "/");
        await JsRuntimeRepo.InvokeVoidAsync("clipboardCopy.copyText", _link);
        SnackBarRepo.Add($"Ссылка `{_link}` скопирована в буфер обмена", Severity.Info, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
    }

    void OnChipClosed(MudChip<NLogRecordModelDB> chip)
    {
        NLogRecordModelDB? el = favoritesRecords.FirstOrDefault(x => x.Id == chip.Value?.Id);
        if (el is not null)
            favoritesRecords.Remove(el);
    }

    void SelectRow(NLogRecordModelDB _row)
    {
        NLogRecordModelDB? el = favoritesRecords.FirstOrDefault(x => x.Id == _row.Id);
        selectedRecord = 0;
        if (el is not null)
            favoritesRecords.Remove(el);
        else
            favoritesRecords.Add(_row);
    }

    async Task ReloadTable()
    {
        if (table is null)
            return;

        await SetBusyAsync();
        await table.ReloadServerData();
        await SetBusyAsync(false);
    }

    private void PageChanged(int i)
    {
        table?.NavigateTo(i - 1);
    }

    static string GetClassLevel(string? recordLevel)
    {
        if (string.IsNullOrWhiteSpace(recordLevel))
            return "";

        if (recordLevel.StartsWith("I", StringComparison.OrdinalIgnoreCase))
            return "text-info";

        if (recordLevel.StartsWith("W", StringComparison.OrdinalIgnoreCase))
            return "text-warning";

        if (recordLevel.StartsWith("E", StringComparison.OrdinalIgnoreCase))
            return "text-danger";

        return "";
    }

    string[]? ApplicationsFilter
    {
        get
        {
            List<string> _res = [];

            if (ApplicationsFilterSet is not null)
                _res.AddRange(ApplicationsFilterSet);

            if (ApplicationsAvailable is not null)
                _res.AddRange(ApplicationsAvailable.GetSelected());
            _res = [.. _res.Distinct()];
            return _res.Count == 0 ? null : [.. _res];
        }
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<NLogRecordModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<LogsSelectRequestModel> req = new()
        {
            Payload = new()
            {
                StartAt = DateRangeBind.Start,
                FinalOff = DateRangeBind.End,
                ApplicationsFilter = ApplicationsFilter,
                ContextsPrefixesFilter = ContextsPrefixesAvailable is null ? null : [.. ContextsPrefixesAvailable.GetSelected()],
                LevelsFilter = LevelsAvailable is null ? null : [.. LevelsAvailable.GetSelected()],
                LoggersFilter = LoggersAvailable is null ? null : [.. LoggersAvailable.GetSelected()],
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
        };

        TPaginationResponseStandardModel<NLogRecordModelDB> selector = default!;
        TResponseModel<LogsMetadataResponseModel> md = default!;
        await SetBusyAsync(token: token);

        await Task.WhenAll([Task.Run(async () => selector = await LogsRepo.LogsSelectAsync(req, token)),
                            Task.Run(async () => md = await LogsRepo.MetadataLogsAsync(new() { StartAt = DateRangeBind.Start, FinalOff = DateRangeBind.End }))]);

        _metaData = md.Response;

        await SetBusyAsync(false, token);
        tableItems = selector.Response;
        return new TableData<NLogRecordModelDB>() { TotalItems = selector.TotalRowsCount, Items = selector.Response };
    }
}