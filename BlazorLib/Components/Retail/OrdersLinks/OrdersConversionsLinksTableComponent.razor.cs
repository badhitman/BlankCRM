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


    async Task DeleteRow((int orderId, int otherDocId) rowLinkId)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        if (!CanDeleteRow(rowLinkId))
            return;

        OrderConversionModel req = new()
        {
            OrderDocumentId = initDeleteRow!.Value.orderId,
            ConversionDocumentId = initDeleteRow.Value.otherDocId,
        };
        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteConversionOrderLinkDocumentRetailAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = req });
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

        if (element is ConversionOrderRetailLinkModelDB other)
        {
            ConversionOrderRetailLinkModelDB req = new()
            {
                AmountPayment = other.AmountPayment,
                Id = other.Id,
                ConversionDocumentId = other.ConversionDocumentId,
                OrderDocumentId = other.OrderDocumentId,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdateConversionOrderLinkDocumentRetailAsync(new()
            {
                Payload = req,
                SenderActionUserId = CurrentUserSession.UserId
            });

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

        if (element is ConversionOrderRetailLinkModelDB other)
            other.AmountPayment = elementBeforeEdit.AmountPayment;
    }

    void IncludeExistConversionOpenDialog() => _visibleIncludeExistConversion = true;

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

        if (ConversionId <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreateConversionOrderLinkDocumentRetailAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                ConversionDocumentId = ConversionId,
                OrderDocumentId = tableRow.Item.Id,
                AmountPayment = tableRow.Item.Rows is null || tableRow.Item.Rows.Count == 0
                 ? 0
                 : tableRow.Item.Rows.Sum(x => x.Amount)
            }
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async void SelectConversionRowAction(TableRowClickEventArgs<WalletConversionRetailDocumentModelDB> tableRow)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        _visibleIncludeExistConversion = false;

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

        TResponseModel<int> res = await RetailRepo.CreateConversionOrderLinkDocumentRetailAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                ConversionDocumentId = tableRow.Item.Id,
                OrderDocumentId = OrderParent.Id,
                AmountPayment = tableRow.Item.ToWalletSum,
            }
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

        if (OrderParent is not null && OrderParent.Id > 0)
            req.Payload.OrdersIds = [OrderParent.Id];

        if (ConversionId > 0)
            req.Payload.ConversionsIds = [ConversionId];

        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<ConversionOrderRetailLinkModelDB> res = await RetailRepo.SelectConversionsOrdersDocumentsLinksRetailAsync(req, token);

        if (res.Response is not null && res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Select(x => x.ConversionDocument!.FromWallet!.UserIdentityId).Union(res.Response.Select(x => x.ConversionDocument!.ToWallet!.UserIdentityId))]);

        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<ConversionOrderRetailLinkModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<ConversionOrderRetailLinkModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}