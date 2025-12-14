////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// OrderStatusesTableComponent
/// </summary>
public partial class OrderStatusesTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public DocumentRetailModelDB Document { get; set; }


    DateTime? createdDate, editDate;
    StatusesDocumentsEnum? newStatus;
    StatusesDocumentsEnum editStatus;
    string? createdName, editName;
    int? editRowId;

    MudTable<OrderStatusRetailDocumentModelDB>? tableRef;
    OrderStatusRetailDocumentModelDB? elementBeforeEdit;

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

        ResponseBaseModel res = await RetailRepo.DeleteOrderStatusDocumentAsync(initDeleteRowStatusId.Value);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        initDeleteRowStatusId = null;
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async void ItemHasBeenCommitted(object element)
    {
        initDeleteRowStatusId = null;

        if (!editDate.HasValue || editDate == default || !editRowId.HasValue)
            return;

        if (element is OrderStatusRetailDocumentModelDB other)
        {
            OrderStatusRetailDocumentModelDB req = new()
            {
                DateOperation = editDate.Value,
                OrderDocumentId = Document.Id,
                Name = editName ?? "",
                StatusDocument = editStatus,
                Id = editRowId.Value,
            };
            await SetBusyAsync();
            ResponseBaseModel res = await RetailRepo.UpdateOrderStatusDocumentAsync(req);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (!res.Success())
            {
                await SetBusyAsync(false);
                return;
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
        initDeleteRowStatusId = null;

        if (element is OrderStatusRetailDocumentModelDB other)
        {
            editDate = other.DateOperation;
            editStatus = other.StatusDocument;
            editName = other.Name;
            editRowId = other.Id;

            elementBeforeEdit = new()
            {
                DateOperation = other.DateOperation,
                Name = other.Name,
                StatusDocument = other.StatusDocument,
            };
        }
    }

    void ResetItemToOriginalValues(object element)
    {
        initDeleteRowStatusId = null;

        if (elementBeforeEdit is null)
            return;

        if (element is OrderStatusRetailDocumentModelDB other)
        {
            other.DateOperation = elementBeforeEdit.DateOperation;
            other.Name = elementBeforeEdit.Name;
            other.StatusDocument = elementBeforeEdit.StatusDocument;

            elementBeforeEdit = null;
        }

        editDate = null;
    }

    async Task AddNewStatus()
    {
        initDeleteRowStatusId = null;

        if (!createdDate.HasValue || createdDate == default || !newStatus.HasValue)
            return;

        OrderStatusRetailDocumentModelDB req = new()
        {
            DateOperation = createdDate.Value,
            OrderDocumentId = Document.Id,
            Name = createdName ?? "",
            StatusDocument = newStatus.Value,
            OrderDocument = Document
        };
        await SetBusyAsync();
        TResponseModel<int> res = await RetailRepo.CreateOrderStatusDocumentAsync(req);
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

    async Task<TableData<OrderStatusRetailDocumentModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req = new()
        {
            Payload = new()
            {
                OrderDocumentId = Document.Id
            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<OrderStatusRetailDocumentModelDB> res = await RetailRepo.SelectOrderDocumentStatusesAsync(req, token);
        await SetBusyAsync(false, token);
        return new TableData<OrderStatusRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}