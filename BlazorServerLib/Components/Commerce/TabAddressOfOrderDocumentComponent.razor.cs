﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.HelpDesk;
using Microsoft.AspNetCore.Components;
using SharedLib;
using BlazorLib;
using BlazorWebLib.Components.Rubrics;

namespace BlazorWebLib.Components.Commerce;

/// <summary>
/// TabAddressOfOrderDocumentComponent
/// </summary>
public partial class TabAddressOfOrderDocumentComponent : OffersTableBaseComponent
{
    [Inject]
    IStorageTransmission StorageTransmissionRepo { get; set; } = default!;

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


    AddRowToOrderDocumentComponent? addingDomRef;
    RowOfOrderDocumentModelDB? elementBeforeEdit;
    RubricSelectorComponent? ref_rubric;
    List<RubricStandardModel>? RubricMetadataShadow;
    bool _showingPriceSelectorOrder;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();

        List<Task> tasks = [
            Task.Run(async () => { TResponseModel<List<RubricStandardModel>> res = await HelpDeskRepo.RubricReadAsync(0); SnackbarRepo.ShowMessagesResponse(res.Messages); RubricMetadataShadow = res.Response; }),
            CacheRegistersUpdate(offers: CurrentTab.Rows!.Select(x => x.OfferId).ToArray(),goods: [],CurrentTab.WarehouseId, true),
            Task.Run(async () => { TResponseModel<bool?> showingPriceSelectorOrder = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ShowingPriceSelectorOrder); _showingPriceSelectorOrder = showingPriceSelectorOrder.Response == true; if (!showingPriceSelectorOrder.Success()) SnackbarRepo.ShowMessagesResponse(showingPriceSelectorOrder.Messages); }) ];

        await Task.WhenAll(tasks);

        await SetBusyAsync(false);

        if (RubricMetadataShadow is not null && RubricMetadataShadow.Count != 0)
        {
            RubricStandardModel current_element = RubricMetadataShadow.Last();
            if (ref_rubric is not null)
            {
                await ref_rubric.OwnerRubricSet(current_element.ParentId ?? 0);
                await ref_rubric.SetRubric(current_element.Id, RubricMetadataShadow);
                ref_rubric.StateHasChangedCall();
            }
        }
    }

    async void SelectOfferAction(OfferModelDB? offer)
    {
        if (CurrentTab.Rows is null || CurrentTab.Rows.Count == 0)
            return;

        await SetBusyAsync();
        await CacheRegistersUpdate(offers: CurrentTab.Rows.Select(x => x.OfferId).ToArray(), goods: [], CurrentTab.WarehouseId, true);
        await SetBusyAsync(false);
    }

    decimal GetMaxValue(RowOfOrderDocumentModelDB ctx)
    {
        return ForceAdding
            ? decimal.MaxValue
            : RegistersCache.Where(x => x.OfferId == ctx.OfferId && x.WarehouseId == CurrentTab.WarehouseId).Sum(x => x.Quantity);
    }

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        CurrentTab.WarehouseId = selectedRubric?.Id ?? 0;

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        if (CurrentTab.Rows is not null)
            InvokeAsync(async () =>
            {
                await CacheRegistersUpdate(offers: CurrentTab.Rows.Select(x => x.OfferId).ToArray(), goods: [], CurrentTab.WarehouseId, true);
                StateHasChanged();
            });
        else
            StateHasChanged();
    }

    /// <inheritdoc/>
    protected override void DeleteRow(int offerId)
    {
        CurrentTab.Rows ??= [];
        CurrentTab.Rows.RemoveAll(x => x.OfferId == offerId);
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        addingDomRef?.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override void AddingOfferAction(OfferActionModel off)
    {
        CurrentTab.Rows ??= [];
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

        StateHasChanged();
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