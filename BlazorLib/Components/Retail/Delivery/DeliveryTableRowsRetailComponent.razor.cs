////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;
using System.Reflection.Metadata;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryTableRowsRetailComponent
/// </summary>
public partial class DeliveryTableRowsRetailComponent : OffersTableBaseComponent
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IJSRuntime JsRuntimeRepo { get; set; } = default!;


    /// <summary>
    /// Если true - тогда можно добавлять офферы, которых нет в остатках.
    /// Если false - тогда для добавления доступны только офферы на остатках
    /// </summary>
    [Parameter]
    public bool ForceAdding { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DeliveryDocumentRetailModelDB Document { get; set; }


    /// <inheritdoc/>
    public AddRowOfferToDocumentComponent? AddingDomRef { get; private set; }
    MudTable<RowOfDeliveryRetailDocumentModelDB>? tableRef;
    RowOfDeliveryRetailDocumentModelDB? elementBeforeEdit;

    IQueryable<KeyValuePair<OfferModelDB, (decimal Quantity, decimal Amount)>> OffersForAddQuery =>
        offersOfOrders
        .Where(x => Document.Rows?.Any(y => y.OfferId == x.Key.Id) != true)
        .AsQueryable();

    readonly Dictionary<OfferModelDB, (decimal Quantity, decimal Amount)> offersOfOrders = [];


    async Task CopyOffers()
    {
        if (Document.Rows is null || Document.Rows.Count == 0)
        {
            SnackBarRepo.Warn("Таблица номенклатуры пуста");
            return;
        }
        string res = "";
        Document.Rows.ForEach(r => { res += $"{r.Offer?.Name} {r.Quantity}; "; });
        res = res.Trim();
        //res = res[..^1];
        await JsRuntimeRepo.InvokeVoidAsync("clipboardCopy.copyText", res);
        SnackBarRepo.Info($"Номенклатура [{res}] скопирована в буфер обмена");
    }

    async Task AddOfferToDeliveryDocument(KeyValuePair<OfferModelDB, (decimal Quantity, decimal Amount)> offerForAddElement)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        Document.Rows ??= [];
        int exist_row = Document.Rows.FindIndex(x => x.OfferId == offerForAddElement.Key.Id);
        if (exist_row < 0)
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
            TResponseModel<int> res = await RetailRepo.CreateRowOfDeliveryDocumentAsync(new()
            {
                Payload = req,
                SenderActionUserId = CurrentUserSession.UserId,
            });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            await ElementsReload();

            if (tableRef is not null)
                await tableRef.ReloadServerData();

            await ElementsReload();
            await GetOrdersOffers();
            await SetBusyAsync(false);

            if (DocumentUpdateHandler is not null)
                DocumentUpdateHandler();

            UpdateData();
            StateHasChanged();
            AddingDomRef?.StateHasChangedCall();
        }
    }

    async Task GetOrdersOffers()
    {
        offersOfOrders.Clear();
        TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req = new()
        {
            PageSize = int.MaxValue,
            Payload = new()
            {
                DeliveriesIds = [Document.Id]
            }
        };
        await SetBusyAsync();
        TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB> res = await RetailRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req);
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
        await CacheRegistersUpdate(_offers: [.. Document.Rows!.Select(x => x.OfferId)], _goods: [], Document.WarehouseId, true);
        List<Task> tasks = [
                Task.Run(base.OnInitializedAsync),
                Task.Run(GetOrdersOffers),
                Task.Run(ElementsReload),
            ];

        await Task.WhenAll(tasks);
        await SetBusyAsync(false);
        if (AddingDomRef is not null)
            await AddingDomRef.CacheRegistersUpdate([], [], _warehouseId: Document.WarehouseId);
    }

    async Task ElementsReload()
    {
        TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req = new()
        {
            Payload = new()
            {
                DeliveryDocumentId = Document.Id,
            },
            PageSize = int.MaxValue,
        };
        await SetBusyAsync();
        TPaginationResponseStandardModel<RowOfDeliveryRetailDocumentModelDB> res = await RetailRepo.SelectRowsOfDeliveryDocumentsAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);

        Document.Rows!.Clear();
        if (res.Response is not null)
            Document.Rows.AddRange(res.Response);

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

    /// <inheritdoc/>
    protected override async void AddingOfferAction(OfferActionModel off)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        Document.Rows ??= [];
        await SetBusyAsync();
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
                TResponseModel<int> res = await RetailRepo.CreateRowOfDeliveryDocumentAsync(new()
                {
                    SenderActionUserId = CurrentUserSession.UserId,
                    Payload = req,
                });
                SnackBarRepo.ShowMessagesResponse(res.Messages);

                await ElementsReload();
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
                TResponseModel<Guid?> res = await RetailRepo.UpdateRowOfDeliveryDocumentAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = Document.Rows[exist_row] });
                SnackBarRepo.ShowMessagesResponse(res.Messages);
                if (res.Response is not null)
                    Document.Version = res.Response.Value;

                await ElementsReload();
                if (tableRef is not null)
                    await tableRef.ReloadServerData();

                await SetBusyAsync(false);
            }
        }

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        UpdateData();
        await CacheRegistersUpdate(_offers: [.. Document.Rows.Select(x => x.OfferId)], _goods: [], Document.WarehouseId, true);

        await SetBusyAsync(false);
        AddingDomRef?.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override async void DeleteRow(int offerId, bool forceDelete = false)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
        if (Document.Id <= 0)
        {
            Document.Rows ??= [];
            Document.Rows.RemoveAll(x => x.OfferId == offerId);
        }
        else
        {
            RowOfDeliveryRetailDocumentModelDB rowOfDocument = Document.Rows!.First(x => x.OfferId == offerId);
            TResponseModel<Guid?> res = await RetailRepo.DeleteRowOfDeliveryDocumentAsync(new()
            {
                SenderActionUserId = CurrentUserSession.UserId,
                Payload = new()
                {
                    DeleteRowOfDeliveryDocumentId = rowOfDocument.Id
                }
            });
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Response is not null)
                Document.Version = res.Response.Value;
        }
        await ElementsReload();
        await GetOrdersOffers();
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        AddingDomRef?.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override async void RowEditCommitHandler(object element)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

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
                    TResponseModel<int> res = await RetailRepo.CreateRowOfDeliveryDocumentAsync(new()
                    {
                        Payload = off,
                        SenderActionUserId = CurrentUserSession.UserId,
                    });
                    SnackBarRepo.ShowMessagesResponse(res.Messages);
                    await ElementsReload();
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
                    TResponseModel<Guid?> res = await RetailRepo.UpdateRowOfDeliveryDocumentAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = off });
                    SnackBarRepo.ShowMessagesResponse(res.Messages);
                    if (res.Response is not null)
                        Document.Version = res.Response.Value;

                    await ElementsReload();
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