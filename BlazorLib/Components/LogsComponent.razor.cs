﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib.Components.Shared;
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


    LogsMetadataResponseModel? _metaData;
    MudDateRangePicker _picker = default!;

    DateRange _dateRangeBind = new(DateTime.Now.AddDays(-1).Date, DateTime.Now.Date);
    DateRange DateRangeBind
    {
        get => _dateRangeBind;
        set { _dateRangeBind = value; InvokeAsync(table.ReloadServerData); }
    }

    #region columns visible
    bool _AllEventProperties;
    bool AllEventProperties
    {
        get => _AllEventProperties;
        set
        {
            _AllEventProperties = value;
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
            InvokeAsync(table.ReloadServerData);
        }
    }
    #endregion

    MudTable<NLogRecordModelDB> table = default!;
    readonly HashSet<NLogRecordModelDB> favoritesRecords = [];

    FiltersUniversalComponent? ContextsPrefixesAvailable = default!;
    FiltersUniversalComponent? ApplicationsAvailable = default!;
    FiltersUniversalComponent? LevelsAvailable = default!;
    FiltersUniversalComponent? LoggersAvailable = default!;

    int selectedRecord;

    string GetCheckBoxIcon(NLogRecordModelDB _row)
    {
        if (favoritesRecords.Any(x => x.Id == _row.Id))
            return Icons.Material.Filled.CheckBox;

        return Icons.Material.Filled.CheckBoxOutlineBlank;
    }

    bool navInit = false;
    async Task OnChipClick(NLogRecordModelDB chip)
    {
        await SetBusyAsync();
        selectedRecord = chip.Id;

        _dateRangeBind = new(null, null);

        ApplicationsAvailable = null;
        ContextsPrefixesAvailable = null;
        LevelsAvailable = null;
        LoggersAvailable = null;

        navInit = true;
        if (table.CurrentPage == 0)
            await table.ReloadServerData();
        else
            table.NavigateTo(0);
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

        if (el is not null)
            favoritesRecords.Remove(el);
        else
            favoritesRecords.Add(_row);
    }

    void CheckedChangedAction()
    {
        InvokeAsync(table.ReloadServerData);
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
        table.NavigateTo(i - 1);
    }

    static string GetClassLevel(string? recordLevel)
    {
        if(string.IsNullOrWhiteSpace(recordLevel))
            return "";

        if (recordLevel.StartsWith("I", StringComparison.OrdinalIgnoreCase))
            return "text-info";

        if (recordLevel.StartsWith("W", StringComparison.OrdinalIgnoreCase))
            return "text-warning";

        if (recordLevel.StartsWith("E", StringComparison.OrdinalIgnoreCase))
            return "text-danger";

        return "";
    }

    TPaginationResponseModel<NLogRecordModelDB>? DirectPage;

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
        TPaginationRequestModel<LogsSelectRequestModel> req = new()
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
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        };
        await SetBusyAsync(token: token);
        if (selectedRecord != 0)
        {
            if (navInit)
            {
                DirectPage = await LogsRepo.GoToPageForRowAsync(new()
                {
                    Payload = selectedRecord,
                    PageNum = state.Page,
                    PageSize = state.PageSize,
                    SortBy = req.SortBy,
                    SortingDirection = req.SortingDirection,
                }, token);
                navInit = false;
                await SetBusyAsync(false, token);
            }

            if (DirectPage is not null)
            {
                if (DirectPage.PageNum == state.Page)
                {
                    int trc = DirectPage.TotalRowsCount;
                    List<NLogRecordModelDB>? items = DirectPage.Response;
                    DirectPage = null;
                    return new TableData<NLogRecordModelDB>() { TotalItems = trc, Items = items };
                }
                else
                    table.NavigateTo(DirectPage.PageNum);
            }
        }

        TPaginationResponseModel<NLogRecordModelDB> selector = default!;
        TResponseModel<LogsMetadataResponseModel> md = default!;

        await Task.WhenAll([
                Task.Run(async () => selector = await LogsRepo.LogsSelectAsync(req, token)),
                Task.Run(async () => md = await LogsRepo.MetadataLogsAsync(new() { StartAt = DateRangeBind.Start, FinalOff = DateRangeBind.End })),
                ]);

        _metaData = md.Response;

        await SetBusyAsync(false, token);
        return new TableData<NLogRecordModelDB>() { TotalItems = selector.TotalRowsCount, Items = selector.Response };
    }
}