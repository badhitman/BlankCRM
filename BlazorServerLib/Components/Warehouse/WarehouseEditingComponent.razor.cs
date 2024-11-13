﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Warehouse;

/// <summary>
/// WarehouseEditingComponent
/// </summary>
public partial class WarehouseEditingComponent : OffersTableBaseComponent
{
    [Inject]
    ICommerceRemoteTransmissionService commRepo { get; set; } = default!;

    [Inject]
    NavigationManager navRepo { get; set; } = default!;

    /// <summary>
    /// Id
    /// </summary>
    [Parameter, EditorRequired]
    public required int Id { get; set; }


    WarehouseDocumentModelDB CurrentDocument = new() { DeliveryData = DateTime.Now, Name = "Новый", NormalizedUpperName = "НОВЫЙ", Rows = [] };
    WarehouseDocumentModelDB editDocument = new() { DeliveryData = DateTime.Now, Name = "Новый", NormalizedUpperName = "НОВЫЙ", Rows = [] };

    AddRowToOrderDocumentComponent? addingDomRef;
    RowOfWarehouseDocumentModelDB? elementBeforeEdit;

    bool CanSave => Id < 1 || !CurrentDocument.Equals(editDocument);
    /// <inheritdoc/>

    protected override async void RowEditCommitHandler(object element)
    {
        if (element is RowOfWarehouseDocumentModelDB _el)
        {
            TResponseModel<int> res = await commRepo.RowForWarehouseUpdate(_el);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
                await ReadDocument();
        }

        base.RowEditCommitHandler(element);
    }

    async Task SaveDocument()
    {
        await SetBusy();
        TResponseModel<int> res = await commRepo.WarehouseUpdate(editDocument);
        await SetBusy(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (editDocument.Id < 1 && res.Response > 0)
        {
            navRepo.NavigateTo($"/warehouse/editing/{res.Response}");
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (Id < 1)
            return;

        await ReadDocument();
    }

    async Task ReadDocument()
    {
        await SetBusy();
        TResponseModel<WarehouseDocumentModelDB[]> res = await commRepo.WarehousesRead([Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response is not null)
            CurrentDocument = res.Response.First();

        editDocument = GlobalTools.CreateDeepCopy(CurrentDocument)!;
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async void AddingOfferAction(OfferGoodActionModel off)
    {
        CurrentDocument.Rows ??= [];
        int exist_row = CurrentDocument.Rows.FindIndex(x => x.OfferId == off.Id);
        TResponseModel<int> res;
        if (exist_row < 0)
        {
            RowOfWarehouseDocumentModelDB _newRow = new()
            {
                GoodsId = off.GoodsId,
                OfferId = off.Id,
                WarehouseDocumentId = CurrentDocument.Id,
                Quantity = off.Quantity,
            };

            await SetBusy();
            res = await commRepo.RowForWarehouseUpdate(_newRow);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            await SetBusy(false);
            if (!res.Success())
                return;

            await ReadDocument();
            addingDomRef?.StateHasChangedCall();
            if (DocumentUpdateHandler is not null)
                DocumentUpdateHandler();
        }
        else
        {
            CurrentDocument.Rows[exist_row].Quantity = +off.Quantity;
            await SetBusy();
            res = await commRepo.RowForWarehouseUpdate(CurrentDocument.Rows[exist_row]);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            await SetBusy(false);
        }

        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();

        StateHasChanged();
        addingDomRef!.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected override void RowEditPreviewHandler(object element)
        => elementBeforeEdit = GlobalTools.CreateDeepCopy((RowOfWarehouseDocumentModelDB)element);

    /// <inheritdoc/>
    protected override void RowEditCancelHandler(object element)
    {
        ((RowOfWarehouseDocumentModelDB)element).Quantity = elementBeforeEdit!.Quantity;
        elementBeforeEdit = null;
    }

    /// <inheritdoc/>
    protected override async void DeleteRow(int offerId)
    {
        CurrentDocument.Rows ??= [];
        RowOfWarehouseDocumentModelDB? currentRow = CurrentDocument.Rows.FirstOrDefault(x => x.OfferId == offerId);
        if (currentRow is null)
        {
            await ReadDocument();
            return;
        }

        if (currentRow.Id < 1)
        {
            CurrentDocument.Rows.RemoveAll(x => x.OfferId == offerId);
            return;
        }

        await SetBusy();
        TResponseModel<bool> res = await commRepo.RowsForOrderDelete([currentRow.Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);

        if (!res.Success())
            return;

        await ReadDocument();
        addingDomRef?.StateHasChangedCall();
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();
    }
}