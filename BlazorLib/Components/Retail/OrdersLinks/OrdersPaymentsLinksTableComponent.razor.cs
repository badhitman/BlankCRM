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
    [Inject]
    protected IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public int PaymentId { get; set; }


    bool
        _visibleIncludeExistPayment,
        _visibleCreateNewPayment;


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
    /// <summary>
    /// UsersCache
    /// </summary>
    protected List<UserInfoModel> UsersCache = [];


    /// <summary>
    /// CacheUsersUpdate
    /// </summary>
    protected async Task CacheUsersUpdate(string[] usersIds)
    {
        usersIds = [.. usersIds.Where(x => !string.IsNullOrWhiteSpace(x) && !UsersCache.Any(y => y.UserId == x)).Distinct()];
        if (usersIds.Length == 0)
            return;

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> users = await IdentityRepo.GetUsersOfIdentityAsync(usersIds);
        SnackBarRepo.ShowMessagesResponse(users.Messages);
        if (users.Success() && users.Response is not null && users.Response.Length != 0)
            lock (UsersCache)
            {
                UsersCache.AddRange(users.Response.Where(x => !UsersCache.Any(y => y.UserId == x.UserId)));
            }

        await SetBusyAsync(false);
    }
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
            OrderDocumentId = tableRow.Item.Id,
            AmountPayment = tableRow.Item.Rows is null || tableRow.Item.Rows.Count == 0
                ? 0
                : tableRow.Item.Rows.Sum(x => x.Amount)
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

        if (OrderParent is null || OrderParent.Id <= 0)
        {
            SnackBarRepo.Error("Не определён контекст заказа (розница)");
            StateHasChanged();
            return;
        }

        await SetBusyAsync();

        TResponseModel<int> res = await RetailRepo.CreatePaymentOrderLinkDocumentAsync(new()
        {
            PaymentDocumentId = tableRow.Item.Id,
            OrderDocumentId = OrderParent.Id,
            AmountPayment = tableRow.Item.Amount,
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

        if (OrderParent is not null && OrderParent.Id > 0)
            req.Payload.OrdersIds = [OrderParent.Id];

        if (PaymentId > 0)
            req.Payload.PaymentsIds = [PaymentId];

        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<PaymentOrderRetailLinkModelDB> res = await RetailRepo.SelectPaymentsOrdersDocumentsLinksAsync(req, token);

        if (res.Response is not null && res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Select(x => x.PaymentDocument!.Wallet!.UserIdentityId).Union(res.Response.Select(x => x.PaymentDocument!.Wallet!.UserIdentityId))]);

        await SetBusyAsync(false, token);

        if (!res.Status.Success())
            return new TableData<PaymentOrderRetailLinkModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<PaymentOrderRetailLinkModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}