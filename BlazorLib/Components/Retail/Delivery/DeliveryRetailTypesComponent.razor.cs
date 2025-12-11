////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryRetailTypesComponent
/// </summary>
public partial class DeliveryRetailTypesComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    List<DeliveryServiceRetailModelDB>? DeliveriesServicesList;

    DeliveryServiceRetailModelDB creatingDeliveryService = new();

    async Task CreateNew()
    {
        await SetBusyAsync();
        TResponseModel<int>? resCreate = await RetailRepo.CreateDeliveryServiceAsync(creatingDeliveryService);
        if(!resCreate.Success())
        {
            SnackBarRepo.ShowMessagesResponse(resCreate.Messages);
            await SetBusyAsync(false);
            return;
        }

        creatingDeliveryService = new();
        TPaginationResponseModel<DeliveryServiceRetailModelDB>? res = await RetailRepo.SelectDeliveryServicesAsync(new () { PageSize = int.MaxValue });
        DeliveriesServicesList = res.Response;
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TPaginationResponseModel<DeliveryServiceRetailModelDB>? res = await RetailRepo.SelectDeliveryServicesAsync(new () { PageSize = int.MaxValue });
        DeliveriesServicesList = res.Response;
        await SetBusyAsync(false);
    }
}