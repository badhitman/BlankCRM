////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.OrdersLinks;

/// <summary>
/// OrdersPaymentsLinksTableComponent
/// </summary>
public partial class OrdersPaymentsLinksTableComponent : OrderLinkBaseComponent<PaymentOrderRetailLinkModelDB>
{
    /// <inheritdoc/>
    [Parameter]
    public int PaymentId { get; set; }


    bool
        _visibleIncludeExistPayment,
        _visibleCreateNewPayment;

    decimal fullScale;


    async Task DeleteRow(int rowLinkId)
    {
        if (!CanDeleteRow(rowLinkId))
            return;

        DeletePaymentOrderLinkRetailDocumentsRequestModel req = new()
        {
            OrderPaymentLinkId = initDeleteRow
        };
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeletePaymentOrderLinkDocumentAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        await DeleteRowFinalize();
    }

    async void ItemHasBeenCommitted(object element)
    {
        if (element is PaymentOrderRetailLinkModelDB other)
        {
            PaymentOrderRetailLinkModelDB req = new()
            {
                AmountPayment = other.AmountPayment,
                Id = other.Id,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdatePaymentOrderLinkDocumentAsync(req);
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

        if (element is PaymentOrderRetailLinkModelDB other)
            other.AmountPayment = elementBeforeEdit.AmountPayment;
    }

    void IncludeExistPaymentOpenDialog() => _visibleIncludeExistPayment = true;

    async void SelectOrderRowAction(TableRowClickEventArgs<DocumentRetailModelDB> tableRow)
    {
        _visibleIncludeOrder = false;

        if (tableRow.Item is null)
        {
            StateHasChanged();
            return;
        }

        if (PaymentId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreatePaymentOrderLinkDocumentAsync(new()
        {
            PaymentDocumentId = PaymentId,
            OrderDocumentId = tableRow.Item.Id
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async void SelectPaymentRowAction(TableRowClickEventArgs<PaymentRetailDocumentModelDB> tableRow)
    {
        _visibleIncludeExistPayment = false;

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

        TResponseModel<int> res = await RetailRepo.CreatePaymentOrderLinkDocumentAsync(new()
        {
            PaymentDocumentId = tableRow.Item.Id,
            OrderDocumentId = OrderId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async Task<TableData<PaymentOrderRetailLinkModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
        };

        if (OrderId > 0)
            req.Payload.OrdersIds = [OrderId];

        if (PaymentId > 0)
            req.Payload.PaymentsIds = [PaymentId];

        await SetBusyAsync(token: token);
        TPaginationResponseModel<PaymentOrderRetailLinkModelDB> res = await RetailRepo.SelectPaymentsOrdersDocumentsLinksAsync(req, token);
        fullScale = res.Response is null || res.Response.Count == 0 ? 0 : res.Response.Sum(x => x.AmountPayment);
        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<PaymentOrderRetailLinkModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<PaymentOrderRetailLinkModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}