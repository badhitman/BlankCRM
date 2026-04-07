////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Reflection.Metadata;

namespace BlazorLib.Components.Retail.Orders;

/// <summary>
/// OrderStatusesTableComponent
/// </summary>
public partial class OrderStatusesTableComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DocumentRetailModelDB Document { get; set; }


    DateTime? createdDate, editDate;
    StatusesDocumentsEnum? newStatus;
    StatusesDocumentsEnum editStatus;
    string? createdName, editName;
    int? editRowId;

    MudTable<OrderStatusRetailDocumentModelDB>? tableRef;
    OrderStatusRetailDocumentModelDB? elementBeforeEdit;

    bool WithoutStatus => newStatus is null;

    bool CannotAdding => newStatus is null || createdDate is null || IsBusyProgress;

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
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        await SetBusyAsync();

        DocumentNewVersionResponseModel res = await RetailRepo.DeleteOrderStatusDocumentAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                DeleteOrderStatusDocumentId = initDeleteRowStatusId.Value
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        initDeleteRowStatusId = null;
        if (res.DocumentNewVersion.HasValue)
            Document.Version = res.DocumentNewVersion.Value;

        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async void ItemHasBeenCommitted(object element)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
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
            TResponseModel<Guid?> res = await RetailRepo.UpdateOrderStatusDocumentAsync(new()
            {
                SenderActionUserId = CurrentUserSession.UserId,
                Payload = req
            });
            if (res.Response is not null)
                Document.Version = res.Response.Value;

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
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
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
        DocumentNewVersionResponseModel res = await RetailRepo.CreateOrderStatusDocumentAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = req
        });

        if (!res.Success())
        {
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            await SetBusyAsync(false);
            return;
        }
        else if (res.DocumentNewVersion.HasValue)
            Document.Version = res.DocumentNewVersion.Value;

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
        TPaginationResponseStandardModel<OrderStatusRetailDocumentModelDB> res = await RetailRepo.SelectOrderDocumentStatusesAsync(req, token);
        await SetBusyAsync(false, token);
        return new TableData<OrderStatusRetailDocumentModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response?.OrderByDescending(x => x.DateOperation).ThenByDescending(os => os.Id) };
    }
}