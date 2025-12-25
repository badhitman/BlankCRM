////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Reports.mmm;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.test;

/// <summary>
/// OffersOfDeliveriesRetailReportComponent
/// </summary>
public partial class OffersOfDeliveriesRetailReportComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public MMMWrapperComponent? Owner { get; set; }


    bool _allowSumConflict;
    bool AllowSumsConflict
    {
        get => _allowSumConflict;
        set
        {
            _allowSumConflict = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }


    bool includeUnset;
    MudTable<OffersRetailReportRowModel>? tableRef;

    DateRange? _dateRange;
    DateRange? DateRangeProp
    {
        get => _dateRange;
        set
        {
            _dateRange = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }

    IReadOnlyCollection<DeliveryStatusesEnum> _selectedStatuses = [];
    IReadOnlyCollection<DeliveryStatusesEnum> SelectedStatuses
    {
        get => _selectedStatuses;
        set
        {
            _selectedStatuses = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (Owner?.SelectedWeek is not null)
            _dateRange = new()
            {
                Start = Owner.SelectedWeek.Value.Start,
                End = Owner.SelectedWeek.Value.End,
            };
    }

    /// <inheritdoc/>
    public async Task Reload()
    {
        if (Owner?.SelectedWeek is not null)
            _dateRange = new()
            {
                Start = Owner.SelectedWeek.Value.Start,
                End = Owner.SelectedWeek.Value.End,
            };

        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }

    async Task OnChipClicked()
    {
        includeUnset = !includeUnset;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }

    async Task<TableData<OffersRetailReportRowModel>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            SortBy = state.SortLabel,
            Payload = new(),
        };

        if (SelectedStatuses.Count != 0)
            req.Payload.StatusesFilter = [.. SelectedStatuses];

        if (includeUnset)
        {
            req.Payload.StatusesFilter ??= [];
            req.Payload.StatusesFilter.Add(null);
        }

        else if (DateRangeProp is not null)
        {
            req.Payload.Start = DateRangeProp.Start;
            req.Payload.End = DateRangeProp.End;
        }

        await SetBusyAsync(token: token);
        TPaginationResponseModel<OffersRetailReportRowModel> res = await RetailRepo.OffersOfDeliveriesReportRetailAsync(req, token);
        await SetBusyAsync(false, token);

        if (res.Response is null)
            return new TableData<OffersRetailReportRowModel>() { TotalItems = 0, Items = [] };

        // Return the data
        return new TableData<OffersRetailReportRowModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}