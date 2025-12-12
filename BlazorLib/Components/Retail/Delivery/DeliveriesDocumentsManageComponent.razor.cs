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
    public int? ExcludeOrderId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<TableRowClickEventArgs<DeliveryDocumentRetailModelDB>>? RowClickEventHandler { get; set; }

    // 
    IReadOnlyCollection<DeliveryStatusesEnum?> _selectedStatuses = [];
    IReadOnlyCollection<DeliveryStatusesEnum?> SelectedStatuses
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
        };

        if (ExcludeOrderId.HasValue && ExcludeOrderId > 0)
            req.Payload.ExcludeOrderId = ExcludeOrderId.Value;

        if (!string.IsNullOrWhiteSpace(ClientId))
            req.Payload.RecipientsFilterIdentityId = [ClientId];

        if (FilterOrderId.HasValue && FilterOrderId > 0)
            req.Payload.FilterOrderId = FilterOrderId.Value;

        if (SelectedTypes.Count != 0)
            req.Payload.TypesFilter = [.. SelectedTypes];

        // SelectedStatuses

        TPaginationResponseModel<DeliveryDocumentRetailModelDB>? res = await RetailRepo.SelectDeliveryDocumentsAsync(req, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null)
            await CacheUsersUpdate([.. res.Response.Select(x => x.RecipientIdentityUserId)]);

        await SetBusyAsync(false, token);
        return new TableData<DeliveryDocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}