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

    DeliveryStatusesEnum? newStatus;
    DateTime? createdDate;
    string? createdName;
    MudTable<DeliveryStatusRetailDocumentModelDB>? tableRef;

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
        if(!res.Success())
        {
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            await SetBusyAsync(false);
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