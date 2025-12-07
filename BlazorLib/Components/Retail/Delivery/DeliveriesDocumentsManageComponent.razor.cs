////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveriesDocumentsManageComponent
/// </summary>
public partial class DeliveriesDocumentsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter(Name = "ClientId")]
    public string? ClientId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int? FilterOrderId { get; set; }


    IReadOnlyCollection<DeliveryTypesEnum> _selectedTypes = [];
    IReadOnlyCollection<DeliveryTypesEnum> SelectedTypes
    {
        get => _selectedTypes;
        set
        {
            _selectedTypes = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }
    MudTable<DeliveryDocumentRetailModelDB>? tableRef;
    bool _visible;
    readonly DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        MaxWidth = MaxWidth.ExtraLarge,
        CloseButton = true,
    };


    void CreateNewDeliveryOpenDialog()
    {
        _visible = true;
    }

    async Task<TableData<DeliveryDocumentRetailModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req = new()
        {
            Payload = new(),
        };

        if (!string.IsNullOrWhiteSpace(ClientId))
            req.Payload.RecipientsFilterIdentityId = [ClientId];

        if (FilterOrderId.HasValue && FilterOrderId > 0)
            req.Payload.FilterOrderId = FilterOrderId.Value;

        if (SelectedTypes.Count != 0)
            req.Payload.TypesFilter = [.. SelectedTypes];

        TPaginationResponseModel<DeliveryDocumentRetailModelDB>? res = await RetailRepo.SelectDeliveryDocumentsAsync(req, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null)
            await CacheUsersUpdate([.. res.Response.Select(x => x.RecipientIdentityUserId)]);

        await SetBusyAsync(false, token);
        return new TableData<DeliveryDocumentRetailModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}