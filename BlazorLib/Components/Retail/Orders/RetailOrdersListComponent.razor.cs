////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// RetailOrdersListComponent
/// </summary>
public partial class RetailOrdersListComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <summary>
    /// Скрыть доставку с указанной доставкой
    /// </summary>
    [Parameter]
    public int? ExcludeDeliveryId { get; set; }

    /// <summary>
    /// Скрыть доставку с указанной оплатой
    /// </summary>
    [Parameter]
    public int? ExcludePaymentId { get; set; }

    /// <summary>
    /// Скрыть доставку с указанным переводом
    /// </summary>
    [Parameter]
    public int? ExcludeConversionId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<TableRowClickEventArgs<DocumentRetailModelDB>>? RowClickEventHandler { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public List<StatusesDocumentsEnum?>? PresetStatusesDocuments { get; set; }


    bool includeUnsetStatus;

    bool _equalSumFilter;
    bool EqualSumFilter
    {
        get => _equalSumFilter;
        set
        {
            _equalSumFilter = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }

    /// <summary>
    /// RubricsCache
    /// </summary>
    protected List<RubricStandardModel> RubricsCache = [];
    /// <summary>
    /// UsersCache
    /// </summary>
    protected List<UserInfoModel> UsersCache = [];

    /// <summary>
    /// PaymentsLinksCache
    /// </summary>
    protected PaymentOrderRetailLinkModelDB[] PaymentsLinksCache = [];
    /// <summary>
    /// DeliveriesLinksCache
    /// </summary>
    protected RetailOrderDeliveryLinkModelDB[] DeliveriesLinksCache = [];
    /// <summary>
    /// ConversionsLinksCache
    /// </summary>
    protected ConversionOrderRetailLinkModelDB[] ConversionsLinksCache = [];

    MudTable<DocumentRetailModelDB>? tableRef;
    bool _visibleCreateNewOrder;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true
    };
    string? searchString;
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

    async Task OnChipClicked()
    {
        includeUnsetStatus = !includeUnsetStatus;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
        await SaveFilters();
    }

    IReadOnlyCollection<StatusesDocumentsEnum> _selectedStatuses = [];
    IReadOnlyCollection<StatusesDocumentsEnum> SelectedStatuses
    {
        get => _selectedStatuses;
        set
        {
            _selectedStatuses = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);

            InvokeAsync(SaveFilters);
        }
    }

    async Task SaveFilters()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
        List<StatusesDocumentsEnum?> _storeStatuses = [.. SelectedStatuses];
        if (includeUnsetStatus)
            _storeStatuses.Add(null);

        await StorageRepo.SaveParameterAsync<StatusesDocumentsEnum?[]?>([.. _storeStatuses], GlobalStaticCloudStorageMetadata.RetailOrdersJournalByStatusesFilters(CurrentUserSession.UserId), true, false);
    }

    static MarkupString NoteGet(string? _html)
    {
        _html ??= "";
        return (MarkupString)_html;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        if (PresetStatusesDocuments is null || PresetStatusesDocuments.Count == 0)
        {
            TResponseModel<StatusesDocumentsEnum?[]?> _readStatusesFilter = await StorageRepo.ReadParameterAsync<StatusesDocumentsEnum?[]?>(GlobalStaticCloudStorageMetadata.RetailOrdersJournalByStatusesFilters(CurrentUserSession.UserId));
            if (_readStatusesFilter.Success() && _readStatusesFilter.Response is not null)
            {
                List<StatusesDocumentsEnum> _markers = [];
                includeUnsetStatus = _readStatusesFilter.Response.Any(x => x is null);
                foreach (StatusesDocumentsEnum _sd in _readStatusesFilter.Response.Where(x => x is not null)!)
                    _markers.Add(_sd);

                _selectedStatuses = [.. _markers];
                if (tableRef is not null)
                    await tableRef.ReloadServerData();
            }
        }
    }

    void CreateNewOrderOpenDialog()
    {
        _visibleCreateNewOrder = true;
    }

    void RowClickEvent(TableRowClickEventArgs<DocumentRetailModelDB> tableRowClickEventArgs)
    {
        if (RowClickEventHandler is not null)
            RowClickEventHandler(tableRowClickEventArgs);
    }

    /// <summary>
    /// CacheUsersUpdate
    /// </summary>
    protected async Task CacheUsersUpdate(string[] usersIds, CancellationToken token)
    {
        usersIds = [.. usersIds.Where(x => !string.IsNullOrWhiteSpace(x) && !UsersCache.Any(y => y.UserId == x)).Distinct()];
        if (usersIds.Length == 0)
            return;

        TResponseModel<UserInfoModel[]> users = await IdentityRepo.GetUsersOfIdentityAsync(usersIds, token);
        SnackBarRepo.ShowMessagesResponse(users.Messages);
        if (users.Success() && users.Response is not null && users.Response.Length != 0)
            lock (UsersCache)
            {
                UsersCache.AddRange(users.Response.Where(x => !UsersCache.Any(y => y.UserId == x.UserId)));
            }
    }

    /// <summary>
    /// CacheRubricsUpdate
    /// </summary>
    protected async Task CacheRubricsUpdate(IEnumerable<int> rubricsIds, CancellationToken token)
    {
        rubricsIds = rubricsIds.Where(x => x > 0 && !RubricsCache.Any(y => y.Id == x)).Distinct();
        if (!rubricsIds.Any())
            return;

        TResponseModel<List<RubricStandardModel>> rubrics = await HelpDeskRepo.RubricsGetAsync(rubricsIds, token);
        SnackBarRepo.ShowMessagesResponse(rubrics.Messages);
        if (rubrics.Success() && rubrics.Response is not null && rubrics.Response.Count != 0)
            lock (RubricsCache)
            {
                RubricsCache.AddRange(rubrics.Response.Where(x => !RubricsCache.Any(y => y.Id == x.Id)));
            }
    }

    async Task<TableData<DocumentRetailModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req = new()
        {
            Payload = new(),
            PageSize = state.PageSize,
            PageNum = state.Page,
            FindQuery = searchString,
        };

        if (!string.IsNullOrWhiteSpace(ClientId))
            req.Payload.BuyersFilterIdentityId = [ClientId];

        if (ExcludeDeliveryId.HasValue && ExcludeDeliveryId > 0)
            req.Payload.ExcludeDeliveryId = ExcludeDeliveryId;

        if (PresetStatusesDocuments is not null && PresetStatusesDocuments.Count != 0)
            req.Payload.StatusesFilter = [.. PresetStatusesDocuments];
        else if (SelectedStatuses.Count != 0)
            req.Payload.StatusesFilter = [.. SelectedStatuses];

        if (includeUnsetStatus)
        {
            req.Payload.StatusesFilter ??= [];
            req.Payload.StatusesFilter.Add(null);
        }

        if (DateRangeProp is not null)
        {
            req.Payload.Start = DateRangeProp.Start;
            req.Payload.End = DateRangeProp.End;
        }

        req.SortBy = state.SortLabel;
        req.SortingDirection = state.SortDirection.Convert();

        if (EqualSumFilter)
            req.Payload.EqualsSumFilter = true;

        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<DocumentRetailModelDB> res = await RetailRepo.SelectRetailDocumentsAsync(req, token);

        if (res.Response is not null)
        {
            IEnumerable<string> _usersIds = res.Response
                .Select(x => x.AuthorIdentityUserId)
                .Union(res.Response.Select(x => x.BuyerIdentityUserId))
                .Distinct();

            List<Task> tasks = [
                Task.Run(async () => { await CacheUsersUpdate([.._usersIds],token); }, token),
                Task.Run(async () => { await CacheRubricsUpdate([.. res.Response.Select(x => x.WarehouseId).Distinct()], token); }, token),
                Task.Run(async () => { await OrdersLinksUpdate(
                    res.Response.SelectMany(x => x.Payments!.Select(y => y.Id)).Distinct(),
                    res.Response.SelectMany(x => x.Deliveries!.Select(y => y.Id)).Distinct(),
                    res.Response.SelectMany(x => x.Conversions!.Select(y => y.Id)).Distinct(), token); }, token),
            ];

            await Task.WhenAll(tasks);
        }

        await SetBusyAsync(false, token: token);
        return new TableData<DocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async Task OrdersLinksUpdate(IEnumerable<int> paymentsLinksIds, IEnumerable<int> deliveriesLinksIds, IEnumerable<int> conversionsLinksIds, CancellationToken token)
    {
        if (paymentsLinksIds.Any())
        {
            TResponseModel<PaymentOrderRetailLinkModelDB[]> resPayments = await RetailRepo.PaymentsOrdersDocumentsLinksGetAsync([.. paymentsLinksIds], token);
            PaymentsLinksCache = resPayments.Response ?? [];
        }
        else
            PaymentsLinksCache = [];

        if (deliveriesLinksIds.Any())
        {
            TResponseModel<RetailOrderDeliveryLinkModelDB[]> resDeliveries = await RetailRepo.DeliveriesOrdersLinksDocumentsReadAsync([.. deliveriesLinksIds], token);
            DeliveriesLinksCache = resDeliveries.Response ?? [];
        }
        else
            DeliveriesLinksCache = [];

        if (conversionsLinksIds.Any())
        {
            TResponseModel<ConversionOrderRetailLinkModelDB[]> resConversions = await RetailRepo.ConversionsOrdersDocumentsLinksReadRetailAsync([.. conversionsLinksIds], token);
            ConversionsLinksCache = resConversions.Response ?? [];
        }
        else
            ConversionsLinksCache = [];
    }

    async void OnSearch(string text)
    {
        searchString = text;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }
}