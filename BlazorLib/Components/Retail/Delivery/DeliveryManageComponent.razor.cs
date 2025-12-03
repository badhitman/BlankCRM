////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Net.Http;
using static MudBlazor.CategoryTypes;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryManageComponent
/// </summary>
public partial class DeliveryManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? FilterClientId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int? FilterOrderId { get; set; }

    async Task<TableData<DeliveryDocumentRetailModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req = new()
        {
            Payload = new()
        };

        if (!string.IsNullOrWhiteSpace(FilterClientId))
            req.Payload.RecipientsFilterIdentityId = [FilterClientId];

        if (FilterOrderId.HasValue && FilterOrderId > 0)
            req.Payload.FilterOrderId = FilterOrderId.Value;

        TPaginationResponseModel<DeliveryDocumentRetailModelDB>? res = await RetailRepo.SelectDeliveryDocumentsAsync(req, token);
        await SetBusyAsync(false, token);
        return new TableData<DeliveryDocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}