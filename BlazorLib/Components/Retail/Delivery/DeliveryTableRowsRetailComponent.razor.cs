////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryTableRowsRetailComponent
/// </summary>
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
    Dictionary<OfferModelDB, (decimal Quantity, decimal Amount)> offersOfOrders = [];
    MudTable<RowOfDeliveryRetailDocumentModelDB>? tableRef;

    IQueryable<KeyValuePair<OfferModelDB, (decimal Quantity, decimal Amount)>> OffersForAddQuery =>
        offersOfOrders
        .Where(x => Document.Rows?.Any(y => y.OfferId == x.Key.Id) != true)
        .AsQueryable();

    /// <inheritdoc/>
    public AddRowToOrderDocumentComponent? AddingDomRef { get; private set; }

    async void AddOfferToDeliveryDocument(KeyValuePair<OfferModelDB, (decimal Quantity, decimal Amount)> offerForAddElement)
    {
        Document.Rows ??= [];
        int exist_row = Document.Rows.FindIndex(x => x.OfferId == offerForAddElement.Key.Id);
        if (exist_row < 0)
        {
            if (Document.Id <= 0)
            {
                Document.Rows.Add(new RowOfDeliveryRetailDocumentModelDB()
                {
                    Document = Document,
                    DocumentId = Document.Id,
                    Nomenclature = offerForAddElement.Key.Nomenclature,
                    NomenclatureId = offerForAddElement.Key.NomenclatureId,
                    Offer = offerForAddElement.Key,
                    OfferId = offerForAddElement.Key.Id,
                    Quantity = offerForAddElement.Value.Quantity,
                    Amount = offerForAddElement.Value.Amount,
                });
            }
            else
            {
                RowOfDeliveryRetailDocumentModelDB req = new()
                {
                    Document = Document,
                    DocumentId = Document.Id,
                    Nomenclature = offerForAddElement.Key.Nomenclature,
                    NomenclatureId = offerForAddElement.Key.NomenclatureId,
                    Offer = offerForAddElement.Key,
                    OfferId = offerForAddElement.Key.Id,
                    Quantity = offerForAddElement.Value.Quantity,
                    Amount = offerForAddElement.Value.Amount,
                };
                await SetBusyAsync();
                TResponseModel<int> res = await RetailRepo.CreateRowOfDeliveryDocumentAsync(req);
                SnackBarRepo.ShowMessagesResponse(res.Messages);
                if (tableRef is not null)
                    await tableRef.ReloadServerData();

                await SetBusyAsync(false);
            }

            if (DocumentUpdateHandler is not null)
                DocumentUpdateHandler();

            UpdateData();
            StateHasChanged();
            AddingDomRef?.StateHasChangedCall();
        }
    }

    async Task GetOrdersOffers()
    {
        TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req = new()
        {
            PageSize = int.MaxValue,
            Payload = new()
            {
                DeliveriesIds = [Document.Id]
            }
        };
        await SetBusyAsync();
        TPaginationResponseModel<RetailOrderDeliveryLinkModelDB> res = await RetailRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null && res.Response.Count != 0)
        {
            TResponseModel<DocumentRetailModelDB[]> readOrders = await RetailRepo.RetailDocumentsGetAsync(new() { Ids = [.. res.Response.Select(x => x.OrderDocumentId)], IncludeDataExternal = true });
            SnackBarRepo.ShowMessagesResponse(readOrders.Messages);

            readOrders.Response?.SelectMany(x => x.Rows!)
                    .GroupBy(x => x.OfferId)
                    .ToList()
                    .ForEach(gRows =>
                    {
                        offersOfOrders.Add(gRows.First().Offer!, !gRows.Any() ? (0, 0) : (gRows.Sum(y => y.Quantity), gRows.Sum(y => y.Amount)));
                    });
        }

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        List<Task> tasks = [
                Task.Run(base.OnInitializedAsync),
                Task.Run(GetOrdersOffers),
            ];

        await Task.WhenAll(tasks);
        await SetBusyAsync(false);
    }

    async Task<TableData<RowOfDeliveryRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req = new()
        {
            Payload = new()
            {
                DeliveryDocumentId = Document.Id,
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB> res = await RetailRepo.SelectRowsOfDeliveryDocumentsAsync(req, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);

        pagedData.Clear();
        if (res.Response is not null)
            pagedData.AddRange(res.Response);

        await SetBusyAsync(false, token);
        return new TableData<RowOfDeliveryRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
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
    protected override async void AddingOfferAction(OfferActionModel off)
    {
        Document.Rows ??= [];
        int exist_row = Document.Rows.FindIndex(x => x.OfferId == off.Id);
        if (exist_row < 0)
        {
            if (Document.Id <= 0)
            {
                Document.Rows.Add(new RowOfDeliveryRetailDocumentModelDB()
                {
                    Document = Document,
                    DocumentId = Document.Id,
                    Nomenclature = off.Nomenclature,
                    NomenclatureId = off.NomenclatureId,
                    Offer = off,
                    OfferId = off.Id,
                    Quantity = off.Quantity,
                    Amount = off.Quantity * off.Price,
                });
            }
            else
            {
                RowOfDeliveryRetailDocumentModelDB req = new()
                {
                    Document = Document,
                    DocumentId = Document.Id,
                    Nomenclature = off.Nomenclature,
                    NomenclatureId = off.NomenclatureId,
                    Offer = off,
                    OfferId = off.Id,
                    Quantity = off.Quantity,
                    Amount = off.Quantity * off.Price,
                };
                await SetBusyAsync();
                TResponseModel<int> res = await RetailRepo.CreateRowOfDeliveryDocumentAsync(req);
                SnackBarRepo.ShowMessagesResponse(res.Messages);
                if (tableRef is not null)
                    await tableRef.ReloadServerData();

                await SetBusyAsync(false);
            }
        }
        else
        {
            Document.Rows[exist_row].Quantity = +off.Quantity;
            Document.Rows[exist_row].Amount = Document.Rows[exist_row].Quantity * off.Price;
            if (Document.Id > 0)
            {
                await SetBusyAsync();
                ResponseBaseModel res = await RetailRepo.UpdateRowOfDeliveryDocumentAsync(Document.Rows[exist_row]);
                SnackBarRepo.ShowMessagesResponse(res.Messages);

                if (tableRef is not null)
                    await tableRef.ReloadServerData();

                await SetBusyAsync(false);
            }
        }

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        UpdateData();
        StateHasChanged();
        AddingDomRef?.StateHasChangedCall();
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
    protected override async void RowEditCommitHandler(object element)
    {
        if (element is RowOfDeliveryRetailDocumentModelDB off)
        {
            int exist_row = Document.Rows!.FindIndex(x => x.OfferId == off.Id);
            if (exist_row < 0)
            {
                if (Document.Id <= 0)
                {
                    Document.Rows.Add(off);
                }
                else
                {
                    await SetBusyAsync();
                    TResponseModel<int> res = await RetailRepo.CreateRowOfDeliveryDocumentAsync(off);
                    SnackBarRepo.ShowMessagesResponse(res.Messages);
                    if (tableRef is not null)
                        await tableRef.ReloadServerData();

                    await SetBusyAsync(false);
                }
            }
            else
            {
                if (Document.Id > 0)
                {
                    await SetBusyAsync();
                    ResponseBaseModel res = await RetailRepo.UpdateRowOfDeliveryDocumentAsync(off);
                    SnackBarRepo.ShowMessagesResponse(res.Messages);

                    if (tableRef is not null)
                        await tableRef.ReloadServerData();

                    await SetBusyAsync(false);
                }
            }

            if (DocumentUpdateHandler is not null)
                DocumentUpdateHandler();

            UpdateData();
            StateHasChanged();
            AddingDomRef!.StateHasChangedCall();
        }
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