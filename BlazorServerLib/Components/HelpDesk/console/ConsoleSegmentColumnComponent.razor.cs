﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.HelpDesk.console;

/// <summary>
/// ConsoleSegmentColumnComponent
/// </summary>
public partial class ConsoleSegmentColumnComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission commRepo { get; set; } = default!;


    /// <summary>
    /// StepIssue
    /// </summary>
    [Parameter, EditorRequired]
    public StatusesDocumentsEnum StepIssue { get; set; }

    /// <summary>
    /// IsLarge
    /// </summary>
    [Parameter, EditorRequired]
    public bool IsLarge { get; set; }

    /// <summary>
    /// UserFilter
    /// </summary>
    [Parameter]
    public string? UserFilter { get; set; }



    string? _searchQuery;
    string? SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            pageNum = 0;
            Issues.Clear();
            InvokeAsync(LoadData);
        }
    }

    static MarkupString MyMarkup(string descr_issue) =>
        new(descr_issue);

    readonly List<IssueHelpDeskModel> Issues = [];
    int totalCount;
    int pageNum = 0;

    async Task LoadData()
    {
        await SetBusyAsync();

        TPaginationResponseModel<IssueHelpDeskModel> res = await HelpDeskRepo.ConsoleIssuesSelectAsync(new TPaginationRequestStandardModel<ConsoleIssuesRequestModel>
        {
            PageNum = pageNum,
            PageSize = 5,
            SortingDirection = DirectionsEnum.Down,
            Payload = new()
            {
                Status = StepIssue,
                SearchQuery = _searchQuery,
                FilterUserId = UserFilter,
                ProjectId = 0,
            }
        });
        IsBusyProgress = false;

        if (res.Response is not null && res.Response.Count != 0)
        {
            totalCount = res.TotalRowsCount;
            Issues.AddRange(res.Response);
            pageNum++;
        }
        await UpdateOrdersCache();
    }

    //
    Dictionary<int, List<OrderDocumentModelDB>> OrdersCache = [];
    Dictionary<int, List<RecordsAttendanceModelDB>> OrdersAttendancesCache = [];
    async Task UpdateOrdersCache()
    {
        int[] issues_ids = Issues.Where(x => !OrdersCache.ContainsKey(x.Id)).Select(x => x.Id).ToArray();
        int[] issues_attendance_ids = Issues.Where(x => !OrdersAttendancesCache.ContainsKey(x.Id)).Select(x => x.Id).ToArray();

        if (issues_ids.Length == 0 && issues_attendance_ids.Length == 0)
            return;

        await SetBusyAsync();

        await Task.WhenAll([
            Task.Run(async () => {
                OrdersByIssuesSelectRequestModel req = new()
                {
                    IssueIds = issues_ids,
                    IncludeExternalData = true
                };

                TResponseModel<OrderDocumentModelDB[]> rest = await commRepo.OrdersByIssuesAsync(req);
                if (rest.Success() && rest.Response is not null && rest.Response.Length != 0)
                {
                    lock(OrdersCache)
                    {
                        foreach (OrderDocumentModelDB ro in rest.Response)
                        {
                            if(!OrdersCache.ContainsKey(ro.HelpDeskId!.Value))
                                OrdersCache.Add(ro.HelpDeskId!.Value, []);

                            OrdersCache[ro.HelpDeskId!.Value].Add(ro);
                        }
                    }
                }
            }),
            Task.Run(async () => {
                OrdersByIssuesSelectRequestModel req = new()
                {
                    IssueIds = issues_attendance_ids,
                    IncludeExternalData = true
                };
                TResponseModel<RecordsAttendanceModelDB[]> restAttendance = await commRepo.OrdersAttendancesByIssuesAsync(req);
                if (restAttendance.Success() && restAttendance.Response is not null && restAttendance.Response.Length != 0)
                {
                    lock(OrdersAttendancesCache)
                    {
                        foreach (RecordsAttendanceModelDB ro in restAttendance.Response)
                        {
                            if(!OrdersAttendancesCache.ContainsKey(ro.HelpDeskId!.Value))
                                OrdersAttendancesCache.Add(ro.HelpDeskId!.Value, []);

                            OrdersAttendancesCache[ro.HelpDeskId!.Value].Add(ro);
                        }
                    }
                }
            })
        ]);

        IsBusyProgress = false;
    }

    string? _luf;
    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender && _luf != UserFilter)
        {
            pageNum = 0;
            Issues.Clear();
            _luf = UserFilter;
            await LoadData();
            StateHasChanged();
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }
}