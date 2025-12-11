////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

public partial class DeliveryTableRowsRetailComponent : OffersTableBaseComponent
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <summary>
    /// Если true - тогда можно добавлять офферы, которых нет в остатках.
    /// Если false - тогда для добавления доступны только офферы на остатках
    /// </summary>
    [Parameter]
    public bool ForceAdding { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public DeliveryDocumentRetailModelDB Document { get; set; }


    List<RowOfDeliveryRetailDocumentModelDB> pagedData = [];
    RowOfDeliveryRetailDocumentModelDB? elementBeforeEdit;

    /// <inheritdoc/>
    public AddRowToOrderDocumentComponent? AddingDomRef { get; private set; }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (Document.Id > 0)
        {
            TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req = new()
            {
                Payload = new()
                {
                    DeliveryDocumentId = Document.Id,
                }
            };
            await SetBusyAsync();
            TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB> res = await RetailRepo.SelectRowsOfDeliveryDocumentsAsync(req);
            SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
            if (res.Response is not null)
            {
                Document.Rows = res.Response;
            }
            await SetBusyAsync(false);
        }
    }

    /// <inheritdoc/>
    public void UpdateData()
    {
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        if (Document.Rows is not null)
            InvokeAsync(async () =>
            {
                await CacheRegistersUpdate(offers: [.. Document.Rows.Select(x => x.OfferId)], goods: [], Document.WarehouseId, true);
                StateHasChanged();
            });
        else
            StateHasChanged();

        AddingDomRef?.StateHasChangedCall();
    }

    decimal GetMaxValue(RowOfDeliveryRetailDocumentModelDB ctx)
    {
        return ForceAdding
            ? decimal.MaxValue
            : RegistersCache.Where(x => x.OfferId == ctx.OfferId && x.WarehouseId == Document?.WarehouseId).Sum(x => x.Quantity);
    }

    async void SelectOfferAction(OfferModelDB? offer)
    {
        if (Document.Rows is null || Document.Rows.Count == 0)
            return;

        await SetBusyAsync();
        await CacheRegistersUpdate(offers: [.. Document.Rows.Select(x => x.OfferId)], goods: [], Document.WarehouseId, true);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override void AddingOfferAction(OfferActionModel off)
    {
        Document.Rows ??= [];
        int exist_row = Document.Rows.FindIndex(x => x.OfferId == off.Id);
        if (exist_row < 0)
            Document.Rows.Add(new RowOfDeliveryRetailDocumentModelDB()
            {
                Document = Document,
                DocumentId = Document.Id,
                Nomenclature = off.Nomenclature,
                NomenclatureId = off.NomenclatureId,
                Offer = off,
                OfferId = off.Id,
                Quantity = off.Quantity,
            });
        else
            Document.Rows[exist_row].Quantity = +off.Quantity;

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();
        UpdateData();
        StateHasChanged();
        AddingDomRef!.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override void DeleteRow(int offerId)
    {
        Document.Rows ??= [];
        Document.Rows.RemoveAll(x => x.OfferId == offerId);
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        AddingDomRef?.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override void RowEditPreviewHandler(object element)
        => elementBeforeEdit = GlobalTools.CreateDeepCopy((RowOfDeliveryRetailDocumentModelDB)element);

    /// <inheritdoc/>
    protected override void RowEditCancelHandler(object element)
    {
        ((RowOfDeliveryRetailDocumentModelDB)element).Quantity = elementBeforeEdit!.Quantity;
        elementBeforeEdit = null;
    }
}