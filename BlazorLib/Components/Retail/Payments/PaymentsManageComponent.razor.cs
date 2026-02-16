////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Collections.Generic;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Retail.Payments;

/// <summary>
/// PaymentsManageComponent
/// </summary>
public partial class PaymentsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<TableRowClickEventArgs<PaymentRetailDocumentModelDB>>? RowClickEventHandler { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public DocumentRetailModelDB? ExcludeOrder { get; set; }


    MudTable<PaymentRetailDocumentModelDB>? _tableRef;
    List<RubricNestedModel> AllPaymentsTypes = [];
    /// <summary>
    /// RubricsCache
    /// </summary>
    protected List<RubricStandardModel> RubricsCache = [];

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

    IReadOnlyCollection<int> _selectedPaymentsTypes = [];
    IReadOnlyCollection<int> SelectedPaymentsTypes
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

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        string ctx = Path.Combine(Routes.PAYMENTS_CONTROLLER_NAME, Routes.TYPES_CONTROLLER_NAME);
        await SetBusyAsync();
        AllPaymentsTypes = await RubricsRepo.RubricsChildListAsync(new() { ContextName = ctx });
        if (AllPaymentsTypes.Count != 0)
            RubricsCache.AddRange(AllPaymentsTypes.Select(x => new RubricStandardModel()
            {
                ContextName = ctx,
                Description = x.Description,
                IsDisabled = x.IsDisabled,
                CreatedAtUTC = x.CreatedAtUTC,
                Id = x.Id,
                LastUpdatedAtUTC = x.LastUpdatedAtUTC,
                Name = x.Name,
                ParentId = x.ParentId,
                SortIndex = x.SortIndex,
                ProjectId = x.ProjectId,
                NormalizedNameUpper = x.Name?.ToUpper()
            }));

        await SetBusyAsync(false);
    }

    void CreateNewOrderOpenDialog()
    {
        _visible = true;
    }

    /// <summary>
    /// CacheRubricsUpdate
    /// </summary>
    protected async Task CacheRubricsUpdate(IEnumerable<int> rubricsIds)
    {
        rubricsIds = rubricsIds.Where(x => x > 0 && !RubricsCache.Any(y => y.Id == x)).Distinct();
        if (!rubricsIds.Any())
            return;

        await SetBusyAsync();
        TResponseModel<List<RubricStandardModel>> rubrics = await RubricsRepo.RubricsGetAsync([.. rubricsIds]);
        SnackBarRepo.ShowMessagesResponse(rubrics.Messages);
        if (rubrics.Success() && rubrics.Response is not null && rubrics.Response.Count != 0)
            lock (RubricsCache)
            {
                RubricsCache.AddRange(rubrics.Response.Where(x => !RubricsCache.Any(y => y.Id == x.Id)));
            }

        await SetBusyAsync(false);
    }

    void RowClickEvent(TableRowClickEventArgs<PaymentRetailDocumentModelDB> tableRowClickEventArgs)
    {
        if (RowClickEventHandler is not null)
            RowClickEventHandler(tableRowClickEventArgs);
    }

    async Task<TableData<PaymentRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            SortBy = state.SortLabel,
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

        if (ExcludeOrder is not null && ExcludeOrder.Id > 0)
            req.Payload.ExcludeOrderId = ExcludeOrder.Id;

        TPaginationResponseStandardModel<PaymentRetailDocumentModelDB>? res = await RetailRepo.SelectPaymentsDocumentsAsync(req, token);

        if (res.Response is not null)
        {
            await CacheUsersUpdate([.. res.Response.Select(x => x.Wallet!.UserIdentityId)]);
            await CacheRubricsUpdate([.. res.Response.Select(x => x.TypePaymentId)]);
        }

        await SetBusyAsync(false, token: token);

        return new TableData<PaymentRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}