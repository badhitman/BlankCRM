////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using BlazorLib.Components.Commerce;
using Microsoft.AspNetCore.Components;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Warehouse;

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
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;


    /// <summary>
    /// Id
    /// </summary>
    [Parameter, EditorRequired]
    public required int Id { get; set; }


    Dictionary<string, object> editorConf = default!;
    string images_upload_url = default!;

    WarehouseDocumentModelDB CurrentDocument = new() { DeliveryDate = DateTime.Now, Name = "Новый", NormalizedUpperName = "НОВЫЙ", Rows = [] };
    WarehouseDocumentModelDB editDocument = new() { DeliveryDate = DateTime.Now, Name = "Новый", NormalizedUpperName = "НОВЫЙ", Rows = [] };

    AddRowOfferToDocumentComponent? addingDomRef;
    RowOfWarehouseDocumentModelDB? elementBeforeEdit;

    MudNumericField<decimal>? _mudQuantityRef;
    MudNumericField<decimal>? mudQuantityRef
    {
        get => _mudQuantityRef;
        set
        {
            _mudQuantityRef = value;
            //if (value is not null)
            //    InvokeAsync(async () => await value.FocusAsync());
            //if (_mudQuantityRef is not null)
            //    _mudQuantityRef.ForceUpdate();
        }
    }

    bool CantSave => Id > 0 && CurrentDocument.Equals(editDocument);

    void IncomingRubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        editDocument.WarehouseId = selectedRubric?.Id ?? 0;
        StateHasChanged();
    }

    void WriteOffRubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        editDocument.WritingOffWarehouseId = selectedRubric?.Id ?? 0;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async void RowEditCommitHandler(object element)
    {
        if (element is RowOfWarehouseDocumentModelDB _el)
        {
            if (CurrentUserSession is null)
            {
                SnackBarRepo.Error("CurrentUserSession is null");
                return;
            }

            DocumentNewVersionResponseModel res = await CommRepo.RowForWarehouseDocumentUpdateOrCreateAsync(new() { Payload = _el, SenderActionUserId = CurrentUserSession.UserId });
            if (res.DocumentNewVersion.HasValue)
                editDocument.Version = res.DocumentNewVersion.Value;

            SnackBarRepo.ShowMessagesResponse(res.Messages);
        }
        await ReadDocument();
        base.RowEditCommitHandler(element);
        _mudQuantityRef = null;
    }

    async Task SaveDocument()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();
        TResponseModel<int> res = await CommRepo.WarehouseDocumentUpdateOrCreateAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = editDocument });
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
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
            TResponseModel<List<RubricStandardModel>> res = await HelpDeskRepo.RubricReadWithParentsHierarchyAsync(0);
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            _shouldRender = true;
            await SetBusyAsync(false);
            return;
        }

        await ReadDocument();
        _shouldRender = true;
        await SetBusyAsync(false);

        if (addingDomRef is not null)
            await addingDomRef.CacheRegistersUpdate([], [], editDocument.WarehouseId);

    }

    async Task ReadDocument()
    {
        if (Id < 1)
            return;

        await SetBusyAsync();
        TResponseModel<WarehouseDocumentModelDB[]> res = await CommRepo.WarehousesDocumentsReadAsync([Id]);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response is not null)
            CurrentDocument = res.Response.First();

        editDocument = GlobalTools.CreateDeepCopy(CurrentDocument)!;

        TResponseModel<List<RubricStandardModel>> resShadow = await HelpDeskRepo.RubricReadWithParentsHierarchyAsync(editDocument.WarehouseId);
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(resShadow.Messages);
    }

    /// <inheritdoc/>
    protected override async void AddingOfferAction(OfferActionModel off)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        CurrentDocument.Rows ??= [];
        int exist_row = CurrentDocument.Rows.FindIndex(x => x.OfferId == off.Id);
        DocumentNewVersionResponseModel res;
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
            res = await CommRepo.RowForWarehouseDocumentUpdateOrCreateAsync(new() { Payload = _newRow, SenderActionUserId = CurrentUserSession.UserId });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (res.DocumentNewVersion.HasValue)
                editDocument.Version = res.DocumentNewVersion.Value;

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
            res = await CommRepo.RowForWarehouseDocumentUpdateOrCreateAsync(new() { Payload = CurrentDocument.Rows[exist_row], SenderActionUserId = CurrentUserSession.UserId });
            SnackBarRepo.ShowMessagesResponse(res.Messages);

            if (res.DocumentNewVersion.HasValue)
                editDocument.Version = res.DocumentNewVersion.Value;

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
    {
        elementBeforeEdit = GlobalTools.CreateDeepCopy((RowOfWarehouseDocumentModelDB)element);
    }

    /// <inheritdoc/>
    protected override void RowEditCancelHandler(object element)
    {
        ((RowOfWarehouseDocumentModelDB)element).Quantity = elementBeforeEdit!.Quantity;
        elementBeforeEdit = null;
        _mudQuantityRef = null;
    }

    /// <inheritdoc/>
    protected override async void DeleteRow(int offerId, bool forceDelete = false)
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

        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();
        TResponseModel<Dictionary<int, DeliveryDocumentMetadataModel>> res = await CommRepo.RowsDeleteFromWarehouseDocumentAsync(new()
        {
            Payload = [currentRow.Id],
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        if (!res.Success() || res.Response is null || res.Response.Count == 0)
            return;

        editDocument.Version = res.Response.First().Value.VersionDocument;
        CurrentDocument.Version = editDocument.Version;

        await ReadDocument();
        addingDomRef?.StateHasChangedCall();
        if (DocumentUpdateHandler is not null)
            DocumentUpdateHandler();
    }
}