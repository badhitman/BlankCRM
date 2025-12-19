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
public partial class OrdersDeliveriesLinksTableComponent : OrderLinkBaseComponent<RetailOrderDeliveryLinkModelDB>
{
    /// <inheritdoc/>
    [Parameter]
    public int DeliveryId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public IReadOnlyCollection<StatusesDocumentsEnum>? PresetStatusesForSelect { get; set; }


    bool
        _visibleIncludeExistDelivery,
        _visibleCreateNewDelivery;

    decimal fullWeight;
    IReadOnlyCollection<StatusesDocumentsEnum?> statusesForSelect = [
            null,
            StatusesDocumentsEnum.Reopen,
            StatusesDocumentsEnum.Progress,
            StatusesDocumentsEnum.Created,
            StatusesDocumentsEnum.Pause,
            StatusesDocumentsEnum.Check
        ];


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
        if (element is RetailOrderDeliveryLinkModelDB other)
        {
            RetailOrderDeliveryLinkModelDB req = new()
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

        if (element is RetailOrderDeliveryLinkModelDB other)
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
        TPaginationResponseModel<RowOfRetailOrderDocumentModelDB> rowsForOrder = await RetailRepo.SelectRowsRetailDocumentsAsync(new TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel>()
        {
            Payload = new()
            {
                OrderId = tableRow.Item.Id,
            },
            PageSize = int.MaxValue,
        });
        TResponseModel<int> res = await RetailRepo.CreateDeliveryOrderLinkDocumentAsync(new()
        {
            DeliveryDocumentId = DeliveryId,
            OrderDocumentId = tableRow.Item.Id,
            WeightShipping = rowsForOrder.Response is null || rowsForOrder.Response.Count == 0
                ? 0
                : rowsForOrder.Response.Sum(x => x.Quantity * x.Offer!.Weight)
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

        if (OrderParent is null || OrderParent.Id <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreateDeliveryOrderLinkDocumentAsync(new()
        {
            DeliveryDocumentId = tableRow.Item.Id,
            OrderDocumentId = OrderParent.Id
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async Task<TableData<RetailOrderDeliveryLinkModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
        };

        if (OrderParent is not null && OrderParent.Id > 0)
            req.Payload.OrdersIds = [OrderParent.Id];

        if (DeliveryId > 0)
            req.Payload.DeliveriesIds = [DeliveryId];

        await SetBusyAsync(token: token);
        TPaginationResponseModel<RetailOrderDeliveryLinkModelDB> res = await RetailRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req, token);
        fullWeight = res.Response is null || res.Response.Count == 0 ? 0 : res.Response.Sum(x => x.WeightShipping);
        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<RetailOrderDeliveryLinkModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<RetailOrderDeliveryLinkModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}