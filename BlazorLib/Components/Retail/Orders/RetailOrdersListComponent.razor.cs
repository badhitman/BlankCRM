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
public partial class RetailOrdersListComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


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


    bool includeUnset;

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
        includeUnset = !includeUnset;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
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

        req.SortBy = state.SortLabel;
        req.SortingDirection = state.SortDirection == SortDirection.Descending
            ? DirectionsEnum.Down
            : DirectionsEnum.Up;

        if (EqualSumFilter)
            req.Payload.EqualsSumFilter = true;

        await SetBusyAsync(token: token);
        TPaginationResponseModel<DocumentRetailModelDB> res = await RetailRepo.SelectRetailDocumentsAsync(req, token);

        if (res.Response is not null)
        {
            IEnumerable<string> _usersIds = res.Response
                .Select(x => x.AuthorIdentityUserId)
                .Union(res.Response.Select(x => x.BuyerIdentityUserId))
                .Distinct();

            IEnumerable<int> _rubricsIds = res.Response.Select(x => x.WarehouseId).Distinct();

            List<Task> tasks = [
                Task.Run(async () => { await CacheUsersUpdate([.._usersIds],token); }, token),
                Task.Run(async () => { await CacheRubricsUpdate([.. _rubricsIds], token); }, token),
            ];

            await Task.WhenAll(tasks);
        }

        await SetBusyAsync(false, token: token);
        return new TableData<DocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async void OnSearch(string text)
    {
        searchString = text;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }
}