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

    string? searchString = null;

    //decimal fullWeight;
    readonly IReadOnlyCollection<StatusesDocumentsEnum?> statusesForSelect = [
            null,
            StatusesDocumentsEnum.Reopen,
            StatusesDocumentsEnum.Progress,
            StatusesDocumentsEnum.Created,
            StatusesDocumentsEnum.Pause,
            StatusesDocumentsEnum.Check
        ];


    async Task DeleteRow((int orderId, int otherDocId) rowLinkId)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        if (!CanDeleteRow(rowLinkId))
            return;

        OrderDeliveryModel req = new()
        {
            OrderId = initDeleteRow!.Value.orderId,
            DeliveryId = initDeleteRow.Value.otherDocId,

        };
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteDeliveryOrderLinkDocumentAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        await DeleteRowFinalize();
    }

    async void ItemHasBeenCommitted(object element)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        if (element is RetailOrderDeliveryLinkModelDB other)
        {
            RetailOrderDeliveryLinkModelDB req = new()
            {
                WeightShipping = other.WeightShipping,
                Id = other.Id,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdateDeliveryOrderLinkDocumentAsync(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId });
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
        initDeleteRow = null;
        if (elementBeforeEdit is null)
            return;

        if (element is RetailOrderDeliveryLinkModelDB other)
            other.WeightShipping = elementBeforeEdit.WeightShipping;
    }

    void IncludeExistDeliveryOpenDialog() => _visibleIncludeExistDelivery = true;

    async void SelectOrderRowAction(TableRowClickEventArgs<DocumentRetailModelDB> tableRow)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
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
        TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB> rowsForOrder = await RetailRepo.SelectRowsRetailDocumentsAsync(new TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel>()
        {
            Payload = new()
            {
                OrderId = tableRow.Item.Id,
            },
            PageSize = int.MaxValue,
        });
        TResponseModel<int> res = await RetailRepo.CreateDeliveryOrderLinkDocumentAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                DeliveryDocumentId = DeliveryId,
                OrderDocumentId = tableRow.Item.Id,
                WeightShipping = rowsForOrder.Response is null || rowsForOrder.Response.Count == 0
                ? 0
                : rowsForOrder.Response.Sum(x => x.Quantity * x.Offer!.Weight)
            }
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async void SelectDeliveryRowAction(TableRowClickEventArgs<DeliveryDocumentRetailModelDB> tableRow)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
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
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                DeliveryDocumentId = tableRow.Item.Id,
                OrderDocumentId = OrderParent.Id,
                WeightShipping = tableRow.Item.Rows is null || tableRow.Item.Rows.Count == 0
                ? 0
                : tableRow.Item.Rows.Sum(x => x.Quantity * x.Offer!.Weight)
            }
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
            FindQuery = searchString,
            Payload = new()
        };

        if (OrderParent is not null && OrderParent.Id > 0)
            req.Payload.OrdersIds = [OrderParent.Id];

        if (DeliveryId > 0)
            req.Payload.DeliveriesIds = [DeliveryId];

        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB> res = await RetailRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req, token);
        //fullWeight = res.Response is null || res.Response.Count == 0 ? 0 : res.Response.Sum(x => x.WeightShipping);

        if (res.Response is not null && OrderParent is not null && OrderParent.Id > 0)
            await CacheUsersUpdate([.. res.Response.Select(x => x.DeliveryDocument!.RecipientIdentityUserId)]);

        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<RetailOrderDeliveryLinkModelDB>()
            {
                TotalItems = 0,
                Items = []
            };

        return new TableData<RetailOrderDeliveryLinkModelDB>()
        {
            TotalItems = res.TotalRowsCount,
            Items = res.Response
        };
    }

    void OnSearch(string text)
    {
        searchString = text;
        if (tableRef is not null)
            InvokeAsync(tableRef.ReloadServerData);
    }
}