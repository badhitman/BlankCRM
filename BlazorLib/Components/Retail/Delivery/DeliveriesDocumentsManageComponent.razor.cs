////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveriesDocumentsManageComponent
/// </summary>
public partial class DeliveriesDocumentsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


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

    MudChip<string>? unsetChipRef;
    bool includeUnset;
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

    IReadOnlyCollection<DeliveryTypesEnum> _selectedTypes = [];
    IReadOnlyCollection<DeliveryTypesEnum> SelectedTypes
    {
        get => _selectedTypes;
        set
        {
            _selectedTypes = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
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
            DeliveryId = deliveryDocumentId,
            OrderId = FilterOrderId.Value,
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    async void OnChipClicked()
    {
        includeUnset = !includeUnset;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
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

    async Task<TableData<DeliveryDocumentRetailModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req = new()
        {
            Payload = new(),
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            SortBy = state.SortLabel,
        };

        if (ExcludeOrder is not null && ExcludeOrder.Id > 0)
            req.Payload.ExcludeOrderId = ExcludeOrder.Id;

        if (!string.IsNullOrWhiteSpace(ClientId))
            req.Payload.RecipientsFilterIdentityId = [ClientId];

        if (FilterOrderId.HasValue && FilterOrderId > 0)
            req.Payload.FilterOrderId = FilterOrderId.Value;

        if (_selectedTypes.Count != 0)
            req.Payload.TypesFilter = [.. SelectedTypes];

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

        if (EqualSumFilter)
            req.Payload.EqualSumFilter = EqualSumFilter;

        TPaginationResponseModel<DeliveryDocumentRetailModelDB>? res = await RetailRepo.SelectDeliveryDocumentsAsync(req, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null)
            await CacheUsersUpdate([.. res.Response.Select(x => x.RecipientIdentityUserId)]);

        await SetBusyAsync(false, token);
        return new TableData<DeliveryDocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}