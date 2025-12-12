////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentsManageComponent
/// </summary>
public partial class PaymentsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    MudTable<PaymentRetailDocumentModelDB>? _tableRef;

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


    bool _visible;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true
    };


    void CreateNewOrderOpenDialog()
    {
        _visible = true;
    }

    async Task<TableData<PaymentRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
            {
                PayerFilterIdentityId = ClientId,
            }
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

        TPaginationResponseModel<PaymentRetailDocumentModelDB>? res = await RetailRepo.SelectPaymentsDocumentsAsync(req, token);
        await SetBusyAsync(false, token: token);

        if (res.Response is not null)
            await CacheUsersUpdate([.. res.Response.Select(x => x.Wallet!.UserIdentityId)]);

        return new TableData<PaymentRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}