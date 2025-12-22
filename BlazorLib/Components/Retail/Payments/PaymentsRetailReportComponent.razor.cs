////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentsRetailReportComponent
/// </summary>
public partial class PaymentsRetailReportComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    MudTable<WalletRetailReportRowModel>? _tableRef;

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

    IReadOnlyCollection<PaymentsRetailStatusesEnum> _selectedPaymentsStatuses = [];
    IReadOnlyCollection<PaymentsRetailStatusesEnum> SelectedPaymentsStatuses
    {
        get => _selectedPaymentsStatuses;
        set
        {
            _selectedPaymentsStatuses = value;
            if (_tableRef is not null)
                InvokeAsync(_tableRef.ReloadServerData);
        }
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

        if (SelectedPaymentsStatuses.Count != 0)
            req.Payload.StatusesFilter = [.. SelectedPaymentsStatuses];

        if (DateRangeProp is not null)
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