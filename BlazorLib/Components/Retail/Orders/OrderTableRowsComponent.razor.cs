////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib.Components.Commerce;
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


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public RetailDocumentModelDB Document { get; set; }

    /// <summary>
    /// Если true - тогда можно добавлять офферы, которых нет в остатках.
    /// Если false - тогда для добавления доступны только офферы на остатках
    /// </summary>
    [Parameter]
    public bool ForceAdding { get; set; }


    List<RowOfRetailOrderDocumentModelDB> pagedData = [];
    RowOfRetailOrderDocumentModelDB? elementBeforeEdit;

    /// <inheritdoc/>
    public AddRowToOrderDocumentComponent? AddingDomRef { get; private set; }


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (Document.Id > 0)
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

    decimal GetMaxValue(RowOfRetailOrderDocumentModelDB ctx)
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
            Document.Rows.Add(new RowOfRetailOrderDocumentModelDB()
            {
                Order = Document,
                OrderId = Document.Id,
                Nomenclature = off.Nomenclature,
                NomenclatureId = off.NomenclatureId,
                Offer = off,
                OfferId = off.Id,
                Quantity = off.Quantity,
                Amount = off.Quantity * off.Price,
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
        => elementBeforeEdit = GlobalTools.CreateDeepCopy((RowOfRetailOrderDocumentModelDB)element);

    /// <inheritdoc/>
    protected override void RowEditCancelHandler(object element)
    {
        ((RowOfRetailOrderDocumentModelDB)element).Quantity = elementBeforeEdit!.Quantity;
        elementBeforeEdit = null;
    }
}