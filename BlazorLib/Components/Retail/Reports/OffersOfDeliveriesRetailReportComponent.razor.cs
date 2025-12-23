////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports;

/// <summary>
/// OffersOfDeliveriesRetailReportComponent
/// </summary>
public partial class OffersOfDeliveriesRetailReportComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


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

        if (AllowSumsConflict)
            req.Payload.EqualsSumFilter = AllowSumsConflict;

        if (SelectedStatuses.Count != 0)
            req.Payload.StatusesFilter = [.. SelectedStatuses];

        if (includeUnset)
        {
            req.Payload.StatusesFilter ??= [];
            req.Payload.StatusesFilter.Add(null);
        }

        if (DateRangeProp is not null)
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