////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Retail.Reports.mmm;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Reports.test;

/// <summary>
/// PaymentsRetailReportComponent
/// </summary>
public partial class PaymentsRetailReportComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public MMMYearSelectorComponent? Owner { get; set; }


    MudTable<WalletRetailReportRowModel>? _tableRef;

    bool includeUnset;

    DateRange? _dateRange;
    DateRange? DateRangeProp
    {
        get => _dateRange;
        set
        {
            _dateRange = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
        }
    }

    IReadOnlyCollection<PaymentsRetailTypesEnum> _selectedPaymentsTypes = [];
    IReadOnlyCollection<PaymentsRetailTypesEnum> SelectedPaymentsTypes
    {
        get => _selectedPaymentsTypes;
        set
        {
            _selectedPaymentsTypes = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
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

    async Task OnChipClicked()
    {
        includeUnset = !includeUnset;
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    /// <inheritdoc/>
    public async Task Reload()
    {
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    async Task<TableData<WalletRetailReportRowModel>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            SortBy = state.SortLabel,
            Payload = new(),
        };

        if (SelectedPaymentsTypes.Count != 0)
            req.Payload.TypesFilter = [.. SelectedPaymentsTypes];

        if (includeUnset)
        {
            req.Payload.TypesFilter ??= [];
            req.Payload.TypesFilter.Add(null);
        }

        if (Owner is not null && Owner.SelectedWeek.HasValue)
        {
            req.Payload.NumWeekOfYear = Owner.SelectedWeek.Value.NumWeekOfYear;
        }
        else if (DateRangeProp is not null)
        {
            req.Payload.Start = DateRangeProp.Start;
            req.Payload.End = DateRangeProp.End;
        }

        await SetBusyAsync(token: token);
        TPaginationResponseModel<WalletRetailReportRowModel> res = await RetailRepo.FinancialsReportRetailAsync(req, token);
        await SetBusyAsync(false, token);

        if (res.Response is null)
            return new TableData<WalletRetailReportRowModel>() { TotalItems = 0, Items = [] };

        // Return the data
        return new TableData<WalletRetailReportRowModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}