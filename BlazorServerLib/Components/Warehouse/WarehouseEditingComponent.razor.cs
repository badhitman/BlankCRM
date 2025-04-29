////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.HelpDesk;
using BlazorWebLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorWebLib.Components.Warehouse;

/// <summary>
/// WarehouseEditingComponent
/// </summary>
public partial class WarehouseEditingComponent : OffersTableBaseComponent
{
    [Inject]
    ICommerceTransmission CommRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;


    /// <summary>
    /// Id
    /// </summary>
    [Parameter, EditorRequired]
    public required int Id { get; set; }

    Dictionary<string, object> editorConf = default!;
    string images_upload_url = default!;

    WarehouseDocumentModelDB CurrentDocument = new() { DeliveryDate = DateTime.Now, Name = "Новый", NormalizedUpperName = "НОВЫЙ", Rows = [] };
    WarehouseDocumentModelDB editDocument = new() { DeliveryDate = DateTime.Now, Name = "Новый", NormalizedUpperName = "НОВЫЙ", Rows = [] };
    RubricSelectorComponent? ref_rubric;
    AddRowToOrderDocumentComponent? addingDomRef;
    RowOfWarehouseDocumentModelDB? elementBeforeEdit;
    List<RubricIssueHelpDeskModelDB>? RubricMetadataShadow;

    bool CanSave => Id < 1 || !CurrentDocument.Equals(editDocument);

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        editDocument.WarehouseId = selectedRubric?.Id ?? 0;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async void RowEditCommitHandler(object element)
    {
        if (element is RowOfWarehouseDocumentModelDB _el)
        {
            TResponseModel<int> res = await CommRepo.RowForWarehouseUpdateAsync(_el);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
        }
        await ReadDocument();
        base.RowEditCommitHandler(element);
    }

    async Task SaveDocument()
    {
        await SetBusyAsync();
        TResponseModel<int> res = await CommRepo.WarehouseUpdateAsync(editDocument);
        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (editDocument.Id < 1 && res.Response > 0)
            NavRepo.NavigateTo($"/nomenclature/warehouse/editing/{res.Response}");
        else if (res.Success())
            await ReadDocument();
    }

    bool _shouldRender = true;
    /// <inheritdoc/>
    protected override bool ShouldRender()
    {
        return _shouldRender;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.WAREHOUSE_CONTROLLER_NAME}/{Routes.BODY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={Id}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        await SetBusyAsync();
        _shouldRender = false;
        await base.OnInitializedAsync();
        if (Id < 1)
        {
            TResponseModel<List<RubricIssueHelpDeskModelDB>> res = await HelpDeskRepo.RubricReadAsync(0);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            RubricMetadataShadow = res.Response;
            if (RubricMetadataShadow is not null && RubricMetadataShadow.Count != 0)
            {
                RubricIssueHelpDeskModelDB current_element = RubricMetadataShadow.Last();
                if (ref_rubric is not null)
                {
                    await ref_rubric.OwnerRubricSet(current_element.ParentId ?? 0);
                    await ref_rubric.SetRubric(current_element.Id, RubricMetadataShadow);
                    ref_rubric.StateHasChangedCall();
                }
            }
            _shouldRender = true;
            await SetBusyAsync(false);
            return;
        }

        await ReadDocument();
        _shouldRender = true;
        await SetBusyAsync(false);
    }

    async Task ReadDocument()
    {
        if (Id < 1)
            return;

        await SetBusyAsync();
        TResponseModel<WarehouseDocumentModelDB[]> res = await CommRepo.WarehousesReadAsync([Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response is not null)
            CurrentDocument = res.Response.First();

        editDocument = GlobalTools.CreateDeepCopy(CurrentDocument)!;

        TResponseModel<List<RubricIssueHelpDeskModelDB>> resShadow = await HelpDeskRepo.RubricReadAsync(editDocument.WarehouseId);
        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(resShadow.Messages);
        RubricMetadataShadow = resShadow.Response;
        if (RubricMetadataShadow is not null && RubricMetadataShadow.Count != 0)
        {
            RubricIssueHelpDeskModelDB current_element = RubricMetadataShadow.Last();
            if (ref_rubric is not null)
            {
                await ref_rubric.OwnerRubricSet(current_element.ParentId ?? 0);
                await ref_rubric.SetRubric(current_element.Id, RubricMetadataShadow);
                ref_rubric.StateHasChangedCall();
            }
        }
    }

    /// <inheritdoc/>
    protected override async void AddingOfferAction(OfferActionModel off)
    {
        CurrentDocument.Rows ??= [];
        int exist_row = CurrentDocument.Rows.FindIndex(x => x.OfferId == off.Id);
        TResponseModel<int> res;
        if (exist_row < 0)
        {
            RowOfWarehouseDocumentModelDB _newRow = new()
            {
                NomenclatureId = off.NomenclatureId,
                OfferId = off.Id,
                WarehouseDocumentId = CurrentDocument.Id,
                Quantity = off.Quantity,
            };

            await SetBusyAsync();
            res = await CommRepo.RowForWarehouseUpdateAsync(_newRow);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            await SetBusyAsync(false);
            if (!res.Success())
                return;

            addingDomRef?.StateHasChangedCall();
            if (DocumentUpdateHandler is not null)
                DocumentUpdateHandler();
        }
        else
        {
            CurrentDocument.Rows[exist_row].Quantity = +off.Quantity;
            await SetBusyAsync();
            res = await CommRepo.RowForWarehouseUpdateAsync(CurrentDocument.Rows[exist_row]);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            await SetBusyAsync(false);
        }

        await ReadDocument();
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

        await SetBusyAsync();
        TResponseModel<bool> res = await CommRepo.RowsForWarehouseDeleteAsync([currentRow.Id]);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        if (!res.Success())
            return;

        await ReadDocument();
        addingDomRef?.StateHasChangedCall();
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();
    }
}