////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveriesDocumentsManageComponent
/// </summary>
public partial class DeliveriesDocumentsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IJSRuntime JsRuntimeRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <summary>
    /// Вывод документов доставки только для указанного заказа
    /// </summary>
    [Parameter]
    public int? FilterOrderId { get; set; }

    /// <summary>
    /// Исключить из вывода документы доставки по номеру заказа
    /// </summary>
    [Parameter]
    public DocumentRetailModelDB? ExcludeOrder { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<TableRowClickEventArgs<DeliveryDocumentRetailModelDB>>? RowClickEventHandler { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public IReadOnlyCollection<DeliveryStatusesEnum?>? PresetStatusesDocuments { get; set; }


    bool _equalSumFilter;
    List<RubricNestedModel> AllDeliveriesTypes = [];
    string? searchString = null;
    /// <summary>
    /// RubricsCache
    /// </summary>
    protected List<RubricStandardModel> RubricsCache = [];
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

    bool includeUnsetStatus;

    IReadOnlyCollection<DeliveryStatusesEnum> _selectedStatuses = [];
    IReadOnlyCollection<DeliveryStatusesEnum> SelectedStatuses
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

    IReadOnlyCollection<int> _selectedTypes = [];
    IReadOnlyCollection<int> SelectedTypes
    {
        get => _selectedTypes;
        set
        {
            _selectedTypes = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);

            InvokeAsync(SaveFilters);
        }
    }
    MudTable<DeliveryDocumentRetailModelDB>? tableRef;
    bool _visibleCreateNewDelivery;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,
    };

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

    int? initDeleteOrderFromDelivery;
    async Task DeleteDeliveryLink(int deliveryDocumentId)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        if (initDeleteOrderFromDelivery is null)
        {
            initDeleteOrderFromDelivery = deliveryDocumentId;
            return;
        }
        initDeleteOrderFromDelivery = null;

        if (!FilterOrderId.HasValue || FilterOrderId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await RetailRepo.DeleteDeliveryOrderLinkDocumentAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                DeliveryId = deliveryDocumentId,
                OrderDocumentId = FilterOrderId.Value,
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
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

        string ctx = Path.Combine(Routes.DELIVERIES_CONTROLLER_NAME, Routes.TYPES_CONTROLLER_NAME);
        AllDeliveriesTypes = await RubricsRepo.RubricsChildListAsync(new() { ContextName = ctx });

        bool _needReload = false;
        await Task.WhenAll([
                Task.Run(async () => {
                    TResponseModel<int?[]?> _readTypesFilter = await StorageRepo.ReadParameterAsync<int?[]?>(GlobalStaticCloudStorageMetadata.RetailDeliveriesJournalByTypesFilters(CurrentUserSession.UserId));
                    if (_readTypesFilter.Success() && _readTypesFilter.Response is not null)
                    {
                        List<int> _markers = [];
                        includeUnsetStatus = _readTypesFilter.Response.Any(x => x is null);
                        foreach (int _sd in _readTypesFilter.Response.Where(x => x is not null)!)
                            _markers.Add(_sd);

                        _selectedTypes = [.. _markers];
                        _needReload = true;
                    }
                 }),
                 Task.Run(async () => {
                    if (PresetStatusesDocuments is null || PresetStatusesDocuments.Count == 0)
                    {
                        TResponseModel<DeliveryStatusesEnum?[]?> _readStatusesFilter = await StorageRepo.ReadParameterAsync<DeliveryStatusesEnum?[]?>(GlobalStaticCloudStorageMetadata.RetailDeliveriesJournalByStatusesFilters(CurrentUserSession.UserId));
                        if (_readStatusesFilter.Success() && _readStatusesFilter.Response is not null)
                        {
                            List<DeliveryStatusesEnum> _markers = [];
                            includeUnsetStatus = _readStatusesFilter.Response.Any(x => x is null);
                            foreach (DeliveryStatusesEnum _sd in _readStatusesFilter.Response.Where(x => x is not null)!)
                                _markers.Add(_sd);

                            _selectedStatuses = [.. _markers];
                        _needReload = true;
                        }
                    }
                 })
           ]);

        if (_needReload && tableRef is not null)
            await tableRef.ReloadServerData();
    }

    async Task SaveFilters()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        List<DeliveryStatusesEnum?> _storeStatuses = [.. SelectedStatuses];
        if (includeUnsetStatus)
            _storeStatuses.Add(null);

        await Task.WhenAll([
                 Task.Run(async () => { await StorageRepo.SaveParameterAsync<DeliveryStatusesEnum?[]>([.. _storeStatuses], GlobalStaticCloudStorageMetadata.RetailDeliveriesJournalByStatusesFilters(CurrentUserSession.UserId), true, false); }),
                 Task.Run(async () => { await StorageRepo.SaveParameterAsync<int[]>([.. _selectedTypes], GlobalStaticCloudStorageMetadata.RetailDeliveriesJournalByTypesFilters(CurrentUserSession.UserId), true, false); })
            ]);
    }

    void OnSearch(string text)
    {
        searchString = text;
        if (tableRef is not null)
            InvokeAsync(tableRef.ReloadServerData);
    }

    async Task OnChipClicked()
    {
        includeUnsetStatus = !includeUnsetStatus;
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SaveFilters();
    }

    void CreateNewDeliveryOpenDialog()
    {
        _visibleCreateNewDelivery = true;
    }

    void RowClickEvent(TableRowClickEventArgs<DeliveryDocumentRetailModelDB> tableRowClickEventArgs)
    {
        if (RowClickEventHandler is not null)
            RowClickEventHandler(tableRowClickEventArgs);
    }

    async Task DownloadFullPrice()
    {
        await SetBusyAsync();
        FileAttachModel res = await RetailRepo.GetDeliveriesJournalFileAsync(GetRequestPayload());
        await SetBusyAsync(false);
        if (res.Data.Length != 0)
        {
            using MemoryStream ms = new(res.Data);
            using DotNetStreamReference streamRef = new(stream: ms);
            await JsRuntimeRepo.InvokeVoidAsync("downloadFileFromStream", res.Name, streamRef);
        }
    }

    async Task<TableData<DeliveryDocumentRetailModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req = new()
        {
            Payload = GetRequestPayload(),
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            SortBy = state.SortLabel,
        };

        TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>? res = await RetailRepo.SelectDeliveryDocumentsAsync(req, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null)
        {
            await CacheUsersUpdate([.. res.Response.Select(x => x.RecipientIdentityUserId)]);
            await CacheRubricsUpdate([.. res.Response.Select(x => x.DeliveryTypeId)]);
        }

        await SetBusyAsync(false, token);
        return new TableData<DeliveryDocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    SelectDeliveryDocumentsRetailRequestModel GetRequestPayload()
    {
        SelectDeliveryDocumentsRetailRequestModel req = new();

        if (ExcludeOrder is not null && ExcludeOrder.Id > 0)
            req.ExcludeOrderId = ExcludeOrder.Id;

        if (!string.IsNullOrWhiteSpace(ClientId))
            req.RecipientsFilterIdentityId = [ClientId];

        if (FilterOrderId.HasValue && FilterOrderId > 0)
            req.FilterOrderId = FilterOrderId.Value;

        if (_selectedTypes.Count != 0)
            req.TypesFilter = [.. SelectedTypes];

        if (PresetStatusesDocuments is not null && PresetStatusesDocuments.Count != 0)
            req.StatusesFilter = [.. PresetStatusesDocuments];
        else if (SelectedStatuses.Count != 0)
            req.StatusesFilter = [.. SelectedStatuses];

        if (includeUnsetStatus)
        {
            req.StatusesFilter ??= [];
            req.StatusesFilter.Add(null);
        }

        if (DateRangeProp is not null)
        {
            req.Start = DateRangeProp.Start;
            req.End = DateRangeProp.End;
        }

        if (EqualSumFilter)
            req.EqualSumFilter = EqualSumFilter;

        return req;
    }
}