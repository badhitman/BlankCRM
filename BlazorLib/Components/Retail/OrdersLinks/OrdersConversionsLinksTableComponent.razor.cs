////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.OrdersLinks;

/// <summary>
/// OrdersConversionsLinksTableComponent
/// </summary>
public partial class OrdersConversionsLinksTableComponent : OrderLinkBaseComponent<ConversionOrderRetailLinkModelDB>
{
    /// <inheritdoc/>
    [Parameter]
    public int ConversionId { get; set; }


    bool
        _visibleIncludeExistConversion,
        _visibleCreateNewConversion;

    decimal fullScale;


    async Task DeleteRow(int rowLinkId)
    {
        if (!CanDeleteRow(rowLinkId))
            return;

        DeleteConversionOrderLinkRetailDocumentsRequestModel req = new()
        {
            OrderConversionLinkId = initDeleteRow
        };
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteConversionOrderLinkDocumentAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        await DeleteRowFinalize();
    }

    async void ItemHasBeenCommitted(object element)
    {
        if (element is ConversionOrderRetailLinkModelDB other)
        {
            ConversionOrderRetailLinkModelDB req = new()
            {
                AmountPayment = other.AmountPayment,
                Id = other.Id,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdateConversionOrderLinkDocumentAsync(req);
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

        if (element is ConversionOrderRetailLinkModelDB other)
            other.AmountPayment = elementBeforeEdit.AmountPayment;
    }

    void IncludeExistConversionOpenDialog() => _visibleIncludeExistConversion = true;

    async void SelectOrderRowAction(TableRowClickEventArgs<DocumentRetailModelDB> tableRow)
    {
        _visibleIncludeOrder = false;

        if (tableRow.Item is null)
        {
            StateHasChanged();
            return;
        }

        if (ConversionId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreateConversionOrderLinkDocumentAsync(new()
        {
            ConversionDocumentId = ConversionId,
            OrderDocumentId = tableRow.Item.Id
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async void SelectConversionRowAction(TableRowClickEventArgs<WalletConversionRetailDocumentModelDB> tableRow)
    {
        _visibleIncludeExistConversion = false;

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

        TResponseModel<int> res = await RetailRepo.CreateConversionOrderLinkDocumentAsync(new()
        {
            ConversionDocumentId = tableRow.Item.Id,
            OrderDocumentId = OrderId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }


    async Task<TableData<ConversionOrderRetailLinkModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new()
        };

        if (OrderId > 0)
            req.Payload.OrdersIds = [OrderId];

        if (ConversionId > 0)
            req.Payload.ConversionsIds = [ConversionId];

        await SetBusyAsync(token: token);
        TPaginationResponseModel<ConversionOrderRetailLinkModelDB> res = await RetailRepo.SelectConversionsOrdersDocumentsLinksAsync(req, token);
        fullScale = res.Response is null || res.Response.Count == 0 ? 0 : res.Response.Sum(x => x.AmountPayment);
        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<ConversionOrderRetailLinkModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<ConversionOrderRetailLinkModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}