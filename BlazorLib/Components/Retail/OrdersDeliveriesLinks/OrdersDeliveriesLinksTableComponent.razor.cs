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


    decimal fullScale;
    bool _visibleIncludeExistDelivery, _visibleIncludeOrder;
    MudTable<RetailDeliveryOrderLinkModelDB>? tableRef;

    readonly static DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,
    };

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