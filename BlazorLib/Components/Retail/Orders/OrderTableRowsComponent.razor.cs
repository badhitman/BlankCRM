////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// OrderTableRowsComponent
/// </summary>
public partial class OrderTableRowsComponent : OffersTableBaseComponent
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DocumentRetailModelDB Document { get; set; }

    /// <summary>
    /// Если true - тогда можно добавлять офферы, которых нет в остатках.
    /// Если false - тогда для добавления доступны только офферы на остатках
    /// </summary>
    [Parameter]
    public bool ForceAdding { get; set; }


    List<RowOfRetailOrderDocumentModelDB> pagedData = [];
    RowOfRetailOrderDocumentModelDB? elementBeforeEdit;
    bool _warehouseReserveForRetailOrder;

    /// <inheritdoc/>
    public AddRowToOrderDocumentComponent? AddingDomRef { get; private set; }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
               .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder);
        _warehouseReserveForRetailOrder = res_WarehouseReserveForRetailOrder.Response == true;

        if (Document.Id > 0)
            await ReloadTableItems();
        else if (Document.Rows is not null)
        {
            await CacheRegistersUpdate(_offers: [.. Document.Rows.Select(x => x.OfferId)], _goods: [], Document.WarehouseId, true);

            if (AddingDomRef is not null)
                AddingDomRef.SetRegistersCache(RegistersCache);
        }
    }

    async Task ReloadTableItems()
    {
        TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req = new()
        {
            Payload = new()
            {
                OrderId = Document.Id,
            }
        };
        await SetBusyAsync();
        TPaginationResponseModel<RowOfRetailOrderDocumentModelDB> res = await RetailRepo.SelectRowsRetailDocumentsAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null)
        {
            Document.Rows = res.Response;
        }

        if (Document.Rows is not null)
        {
            await CacheRegistersUpdate(_offers: [.. Document.Rows.Select(x => x.OfferId)], _goods: [], Document.WarehouseId, true);
            
            if (AddingDomRef is not null)
                AddingDomRef.SetRegistersCache(RegistersCache);
        }
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    public void UpdateData()
    {
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        if (Document.Rows is not null)
            InvokeAsync(async () =>
            {
                await CacheRegistersUpdate(_offers: [.. Document.Rows.Select(x => x.OfferId)], _goods: [], Document.WarehouseId, true);
                AddingDomRef?.SetRegistersCache(RegistersCache);
                StateHasChanged();
            });
        else
            StateHasChanged();

        AddingDomRef?.StateHasChangedCall();
    }

    decimal GetMaxValue(RowOfRetailOrderDocumentModelDB ctx)
    {
        return ForceAdding
            ? decimal.MaxValue
            : RegistersCache.Where(x => x.OfferId == ctx.OfferId && x.WarehouseId == Document?.WarehouseId).Sum(x => x.Quantity);
    }

    /// <inheritdoc/>
    protected override async void RowEditCommitHandler(object element)
    {
        Document.Rows ??= [];

        if (element is RowOfRetailOrderDocumentModelDB rowOrder)
        {
            int exist_row = Document.Rows.FindIndex(x => x.OfferId == rowOrder.OfferId);
            if (exist_row >= 0)
            {

                Document.Rows[exist_row].Quantity = +rowOrder.Quantity;
                Document.Rows[exist_row].WeightOffer = Document.Rows[exist_row].Quantity * rowOrder.Offer!.Weight;
                Document.Rows[exist_row].Amount = Document.Rows[exist_row].Quantity * rowOrder.Offer!.Price;

                if (Document.Id > 0)
                {
                    await SetBusyAsync();
                    TResponseModel<Guid?>? resUpdateRow = await RetailRepo.UpdateRowRetailDocumentAsync(Document.Rows[exist_row]);
                    SnackBarRepo.ShowMessagesResponse(resUpdateRow.Messages);

                    if (resUpdateRow.Success() && resUpdateRow.Response is not null)
                        Document.Version = resUpdateRow.Response.Value;

                    await ReloadTableItems();
                    await SetBusyAsync(false);
                }
            }
        }
        base.RowEditCommitHandler(element);
    }

    /// <inheritdoc/>
    protected override async void AddingOfferAction(OfferActionModel off)
    {
        Document.Rows ??= [];
        await SetBusyAsync();
        int exist_row = Document.Rows.FindIndex(x => x.OfferId == off.Id);
        if (exist_row < 0)
        {
            RowOfRetailOrderDocumentModelDB newOrderElement = new()
            {
                Order = Document,
                OrderId = Document.Id,
                Nomenclature = off.Nomenclature,
                NomenclatureId = off.NomenclatureId,
                Offer = off,
                OfferId = off.Id,
                Quantity = off.Quantity,
                Amount = off.Quantity * off.Price,
                WeightOffer = off.Weight * off.Quantity,
            };
            if (Document.Id == 0)
                Document.Rows.Add(newOrderElement);
            else
            {
                await SetBusyAsync();
                TResponseModel<KeyValuePair<int, Guid>?> resAddingRow = await RetailRepo.CreateRowRetailDocumentAsync(newOrderElement);
                SnackBarRepo.ShowMessagesResponse(resAddingRow.Messages);

                if (resAddingRow.Success() && resAddingRow.Response is not null)
                    Document.Version = resAddingRow.Response.Value.Value;

                await ReloadTableItems();
                await SetBusyAsync(false);
            }
        }
        else
        {
            Document.Rows[exist_row].Quantity = +off.Quantity;
            Document.Rows[exist_row].WeightOffer = Document.Rows[exist_row].Quantity * off.Weight;
            Document.Rows[exist_row].Amount = Document.Rows[exist_row].Quantity * off.Price;

            if (Document.Id > 0)
            {
                await SetBusyAsync();
                TResponseModel<Guid?> resUpdateRow = await RetailRepo.UpdateRowRetailDocumentAsync(Document.Rows[exist_row]);
                SnackBarRepo.ShowMessagesResponse(resUpdateRow.Messages);

                if (resUpdateRow.Success() && resUpdateRow.Response is not null)
                    Document.Version = resUpdateRow.Response.Value;

                await ReloadTableItems();
                await SetBusyAsync(false);
            }
        }

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        UpdateData();
        await CacheRegistersUpdate(_offers: [.. Document.Rows.Select(x => x.OfferId)], _goods: [], Document.WarehouseId, true);

        await SetBusyAsync(false);
        AddingDomRef!.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override async void DeleteRow(int offerId)
    {
        Document.Rows ??= [];
        if (Document.Id <= 0)
            Document.Rows.RemoveAll(x => x.OfferId == offerId);

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        AddingDomRef?.StateHasChangedCall();

        if (Document.Id > 0)
        {
            int exist_row = Document.Rows.FindIndex(x => x.OfferId == offerId);
            if (exist_row >= 0 && Document.Rows[exist_row].Id > 0)
            {
                await SetBusyAsync();
                TResponseModel<Guid?> resDelRow = await RetailRepo.DeleteRowRetailDocumentAsync(Document.Rows[exist_row].Id);
                SnackBarRepo.ShowMessagesResponse(resDelRow.Messages);

                if (resDelRow.Success() && resDelRow.Response is not null)
                    Document.Version = resDelRow.Response.Value;

                await ReloadTableItems();
                await SetBusyAsync(false);
            }
        }
    }

    /// <inheritdoc/>
    protected override void RowEditPreviewHandler(object element)
        => elementBeforeEdit = GlobalTools.CreateDeepCopy((RowOfRetailOrderDocumentModelDB)element);

    /// <inheritdoc/>
    protected override void RowEditCancelHandler(object element)
    {
        ((RowOfRetailOrderDocumentModelDB)element).Quantity = elementBeforeEdit!.Quantity;
        elementBeforeEdit = null;
    }
}