////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// TabAddressOfOrderDocumentComponent
/// </summary>
public partial class TabAddressOfOrderDocumentComponent : OffersTableBaseComponent
{
    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;


    /// <summary>
    /// CurrentTab
    /// </summary>
    [Parameter, EditorRequired]
    public required TabOfficeForOrderModelDb CurrentTab { get; set; }

    /// <summary>
    /// Если true - тогда можно добавлять офферы, которых нет в остатках.
    /// Если false - тогда для добавления доступны только офферы на остатках
    /// </summary>
    [Parameter]
    public bool ForceAdding { get; set; }


    AddRowOfferToDocumentComponent? addingDomRef;
    RowOfOrderDocumentModelDB? elementBeforeEdit;
    bool _showingPriceSelectorOrder;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();

        List<Task> tasks = [
            CacheRegistersUpdate(_offers: [.. CurrentTab.Rows!.Select(x => x.OfferId)],_goods: [],CurrentTab.WarehouseId, true),
            Task.Run(async () => { TResponseModel<bool?> showingPriceSelectorOrder = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ShowingPriceSelectorOrder); _showingPriceSelectorOrder = showingPriceSelectorOrder.Response == true; if (!showingPriceSelectorOrder.Success()) SnackBarRepo.ShowMessagesResponse(showingPriceSelectorOrder.Messages); }) ];

        await Task.WhenAll(tasks);

        await SetBusyAsync(false);
    }

    async void SelectOfferAction(OfferModelDB? offer)
    {
        if (CurrentTab.Rows is null || CurrentTab.Rows.Count == 0)
            return;

        await SetBusyAsync();
        await CacheRegistersUpdate(_offers: [.. CurrentTab.Rows.Select(x => x.OfferId)], _goods: [], CurrentTab.WarehouseId, true);
        await SetBusyAsync(false);
    }

    decimal GetMaxValue(RowOfOrderDocumentModelDB ctx)
    {
        return ForceAdding
            ? decimal.MaxValue
            : RegistersCache.Where(x => x.OfferId == ctx.OfferId && x.WarehouseId == CurrentTab.WarehouseId).Sum(x => x.Quantity);
    }

    void RubricSelectAction(RubricNestedModel? selectedRubric)
    {
        CurrentTab.WarehouseId = selectedRubric?.Id ?? 0;

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        if (CurrentTab.Rows is not null)
            InvokeAsync(async () =>
            {
                await CacheRegistersUpdate(_offers: [.. CurrentTab.Rows.Select(x => x.OfferId)], _goods: [], CurrentTab.WarehouseId, true);
                StateHasChanged();
            });
        else
            StateHasChanged();
    }

    /// <inheritdoc/>
    protected override void DeleteRow(int offerId, bool forceDelete = false)
    {
        CurrentTab.Rows ??= [];
        CurrentTab.Rows.RemoveAll(x => x.OfferId == offerId);
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        addingDomRef?.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override async void AddingOfferAction(OfferActionModel off)
    {
        CurrentTab.Rows ??= [];
        await SetBusyAsync();
        int exist_row = CurrentTab.Rows.FindIndex(x => x.OfferId == off.Id);
        if (exist_row < 0)
            CurrentTab.Rows.Add(new RowOfOrderDocumentModelDB()
            {
                OfficeOrderTab = CurrentTab,
                OfficeOrderTabId = CurrentTab.Id,
                Nomenclature = off.Nomenclature,
                NomenclatureId = off.NomenclatureId,
                Offer = off,
                OfferId = off.Id,
                Order = CurrentTab.Order,
                OrderId = CurrentTab.OrderId,
                Quantity = off.Quantity,
            });
        else
            CurrentTab.Rows[exist_row].Quantity = +off.Quantity;

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        await CacheRegistersUpdate(_offers: [.. CurrentTab.Rows.Select(x => x.OfferId)], _goods: [], CurrentTab.WarehouseId, true);
        await SetBusyAsync(false);
        addingDomRef!.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override void RowEditPreviewHandler(object element)
        => elementBeforeEdit = GlobalTools.CreateDeepCopy((RowOfOrderDocumentModelDB)element);

    /// <inheritdoc/>
    protected override void RowEditCancelHandler(object element)
    {
        ((RowOfOrderDocumentModelDB)element).Quantity = elementBeforeEdit!.Quantity;
        elementBeforeEdit = null;
    }
}