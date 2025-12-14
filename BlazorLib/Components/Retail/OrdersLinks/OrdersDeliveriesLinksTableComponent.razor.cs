////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.OrdersLinks;

/// <summary>
/// OrdersDeliveriesLinksTableComponent
/// </summary>
public partial class OrdersDeliveriesLinksTableComponent : OrderLinkBaseComponent<RetailDeliveryOrderLinkModelDB>
{
    /// <inheritdoc/>
    [Parameter]
    public int DeliveryId { get; set; }


    bool
        _visibleIncludeExistDelivery,
        _visibleCreateNewDelivery;

    decimal fullScale;


    async Task DeleteRow(int rowLinkId)
    {
        if (!CanDeleteRow(rowLinkId))
            return;

        DeleteDeliveryOrderLinkRetailDocumentsRequestModel req = new()
        {
            OrderDeliveryLinkId = initDeleteRow
        };
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteDeliveryOrderLinkDocumentAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        await DeleteRowFinalize();
    }

    async void ItemHasBeenCommitted(object element)
    {
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

            await ItemHasBeenCommittedFinalize();
        }
    }

    void ResetItemToOriginalValues(object element)
    {
        initDeleteRow = 0;
        if (elementBeforeEdit is null)
            return;

        if (element is RetailDeliveryOrderLinkModelDB other)
            other.WeightShipping = elementBeforeEdit.WeightShipping;
    }

    void IncludeExistDeliveryOpenDialog() => _visibleIncludeExistDelivery = true;

    async void SelectOrderRowAction(TableRowClickEventArgs<DocumentRetailModelDB> tableRow)
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