////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.OrdersDeliveriesLinks;

/// <summary>
/// OrdersDeliveriesLinksTableComponent
/// </summary>
public partial class OrdersDeliveriesLinksTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public int OrderId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int DeliveryId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }


    bool
        _visibleIncludeExistDelivery,
        _visibleIncludeOrder,
        _visibleCreateNewOrder,
        _visibleCreateNewDelivery;

    decimal fullScale;
    MudTable<RetailDeliveryOrderLinkModelDB>? tableRef;
    int? initDeleteRow;
    RetailDeliveryOrderLinkModelDB? elementBeforeEdit;


    readonly static DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,
    };

    async Task DeleteRow(int rowStatusId)
    {
        if (initDeleteRow is null)
        {
            initDeleteRow = rowStatusId;
            return;
        }
        if (initDeleteRow != rowStatusId)
        {
            initDeleteRow = null;
            return;
        }
        DeleteDeliveryOrderLinkRetailDocumentsRequestModel req = new()
        {
            OrderDeliveryLinkId = initDeleteRow.Value
        };
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteDeliveryOrderLinkDocumentAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        initDeleteRow = null;
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    void BackupItem(object element)
    {
        initDeleteRow = null;
        if (element is RetailDeliveryOrderLinkModelDB other)
        {
            elementBeforeEdit = GlobalTools.CreateDeepCopy(other)!;
        }
    }

    async void ItemHasBeenCommitted(object element)
    {
        initDeleteRow = null;

        if (element is RetailDeliveryOrderLinkModelDB other)
        {
            RetailDeliveryOrderLinkModelDB req = new()
            {
                WeightShipping = other.WeightShipping,
                Id = other.Id,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdateDeliveryOrderLinkDocumentAsync(req);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (!res.Success())
            {
                await SetBusyAsync(false);
                return;
            }

            if (tableRef is not null)
                await tableRef.ReloadServerData();

            await SetBusyAsync(false);
        }
    }

    void ResetItemToOriginalValues(object element)
    {
        initDeleteRow = null;
        if (elementBeforeEdit is null)
            return;

        if (element is RetailDeliveryOrderLinkModelDB other)
            other.WeightShipping = elementBeforeEdit.WeightShipping;
    }

    void IncludeExistDeliveryOpenDialog()
    {
        _visibleIncludeExistDelivery = true;
    }

    void IncludeExistOrderOpenDialog()
    {
        _visibleIncludeOrder = true;
    }

    async void SelectOrderRowAction(TableRowClickEventArgs<RetailDocumentModelDB> tableRow)
    {
        _visibleIncludeOrder = false;

        if (tableRow.Item is null)
        {
            StateHasChanged();
            return;
        }

        if (DeliveryId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreateDeliveryOrderLinkDocumentAsync(new()
        {
            DeliveryDocumentId = DeliveryId,
            OrderDocumentId = tableRow.Item.Id
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async void SelectDeliveryRowAction(TableRowClickEventArgs<DeliveryDocumentRetailModelDB> tableRow)
    {
        _visibleIncludeExistDelivery = false;

        if (tableRow.Item is null)
        {
            StateHasChanged();
            return;
        }

        if (OrderId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreateDeliveryOrderLinkDocumentAsync(new()
        {
            DeliveryDocumentId = tableRow.Item.Id,
            OrderDocumentId = OrderId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async Task<TableData<RetailDeliveryOrderLinkModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
        };

        if (OrderId > 0)
            req.Payload.OrdersIds = [OrderId];

        if (DeliveryId > 0)
            req.Payload.DeliveriesIds = [DeliveryId];

        await SetBusyAsync(token: token);
        TPaginationResponseModel<RetailDeliveryOrderLinkModelDB> res = await RetailRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req, token);
        fullScale = res.Response is null || res.Response.Count == 0 ? 0 : res.Response.Sum(x => x.WeightShipping);
        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<RetailDeliveryOrderLinkModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<RetailDeliveryOrderLinkModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}