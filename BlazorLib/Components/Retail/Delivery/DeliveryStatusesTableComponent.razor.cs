////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryStatusesTableComponent
/// </summary>
public partial class DeliveryStatusesTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public DeliveryDocumentRetailModelDB Document { get; set; }

    DateTime? createdDate, editDate;
    DeliveryStatusesEnum? newStatus;
    DeliveryStatusesEnum editStatus;
    string? createdName, editName;
    int? editRowId;

    MudTable<DeliveryStatusRetailDocumentModelDB>? tableRef;
    DeliveryStatusRetailDocumentModelDB? elementBeforeEdit;

    int? initDeleteRowStatusId;
    async Task DeleteRow(int rowStatusId)
    {
        if (initDeleteRowStatusId is null)
        {
            initDeleteRowStatusId = rowStatusId;
            return;
        }
        if (initDeleteRowStatusId != rowStatusId)
        {
            initDeleteRowStatusId = null;
            return;
        }

        await SetBusyAsync();

        ResponseBaseModel res = await RetailRepo.DeleteDeliveryStatusDocumentAsync(initDeleteRowStatusId.Value);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        initDeleteRowStatusId = null;
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async void ItemHasBeenCommitted(object element)
    {
        if (!editDate.HasValue || editDate == default || !editRowId.HasValue)
            return;

        if (element is DeliveryStatusRetailDocumentModelDB other)
        {
            DeliveryStatusRetailDocumentModelDB req = new()
            {
                DateOperation = editDate.Value,
                DeliveryDocumentId = editRowId.Value,
                Name = editName ?? "",
                DeliveryStatus = editStatus,
                Id = editRowId.Value,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdateDeliveryStatusDocumentAsync(req);
            if (!res.Success())
            {
                SnackBarRepo.ShowMessagesResponse(res.Messages);
                await SetBusyAsync(false);
            }

            if (tableRef is not null)
                await tableRef.ReloadServerData();

            editStatus = default;
            editDate = null;
            editName = null;
            await SetBusyAsync(false);
        }
    }

    void BackupItem(object element)
    {
        if (element is DeliveryStatusRetailDocumentModelDB other)
        {
            editDate = other.DateOperation;
            editStatus = other.DeliveryStatus;
            editName = other.Name;
            editRowId = other.Id;

            elementBeforeEdit = new()
            {
                DateOperation = other.DateOperation,
                Name = other.Name,
                DeliveryStatus = other.DeliveryStatus,
            };
        }
    }

    void ResetItemToOriginalValues(object element)
    {
        if (elementBeforeEdit is null)
            return;

        if (element is DeliveryStatusRetailDocumentModelDB other)
        {
            other.DateOperation = elementBeforeEdit.DateOperation;
            other.Name = elementBeforeEdit.Name;
            other.DeliveryStatus = elementBeforeEdit.DeliveryStatus;

            elementBeforeEdit = null;
        }

        editDate = null;
    }

    async Task AddNewStatus()
    {
        if (!createdDate.HasValue || createdDate == default || !newStatus.HasValue)
            return;

        DeliveryStatusRetailDocumentModelDB req = new()
        {
            DateOperation = createdDate.Value,
            DeliveryDocumentId = Document.Id,
            Name = createdName ?? "",
            DeliveryStatus = newStatus.Value,
            DeliveryDocument = Document
        };
        await SetBusyAsync();
        TResponseModel<int> res = await RetailRepo.CreateDeliveryStatusDocumentAsync(req);
        if (!res.Success())
        {
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            await SetBusyAsync(false);
            return;
        }

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        newStatus = null;
        createdDate = null;
        createdName = null;
        await SetBusyAsync(false);
    }

    async Task<TableData<DeliveryStatusRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req = new()
        {
            Payload = new()
            {
                DeliveryDocumentId = Document.Id
            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB> res = await RetailRepo.SelectDeliveryStatusesDocumentsAsync(req, token);
        await SetBusyAsync(false, token);
        return new TableData<DeliveryStatusRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}