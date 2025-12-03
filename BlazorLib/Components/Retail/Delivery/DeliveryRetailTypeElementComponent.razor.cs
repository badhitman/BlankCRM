////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Delivery;

/// <summary>
/// DeliveryRetailTypeElementComponent
/// </summary>
public partial class DeliveryRetailTypeElementComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required DeliveryServiceRetailModelDB DeliveryTypeElement { get; set; }


    DeliveryServiceRetailModelDB? _deliveryCopy;

    bool IsEdited => _deliveryCopy is not null && !DeliveryTypeElement.Equals(_deliveryCopy);

    void InitEdit()
    {
        _deliveryCopy = GlobalTools.CreateDeepCopy(DeliveryTypeElement)!;
    }

    void CancelEdit()
    {
        _deliveryCopy = null;
    }

    async Task Save()
    {
        if (_deliveryCopy is null)
        {
            SnackBarRepo.Error("_walletCopy is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel resUpd = await RetailRepo.UpdateDeliveryServiceAsync(_deliveryCopy);
        if (!resUpd.Success())
        {
            await SetBusyAsync(false);
            SnackBarRepo.ShowMessagesResponse(resUpd.Messages);
            return;
        }

        TResponseModel<DeliveryServiceRetailModelDB[]>? resGet = await RetailRepo.DeliveryServicesGetAsync([_deliveryCopy.Id]);
        if (!resGet.Success() || resGet.Response is null || resGet.Response.Length != 1)
        {
            await SetBusyAsync(false);
            SnackBarRepo.ShowMessagesResponse(resUpd.Messages);
            return;
        }

        DeliveryTypeElement.Name = resGet.Response[0].Name;
        DeliveryTypeElement.Description = resGet.Response[0].Description;
        DeliveryTypeElement.IsDisabled = resGet.Response[0].IsDisabled;
        DeliveryTypeElement.LastUpdatedAtUTC = resGet.Response[0].LastUpdatedAtUTC;

        _deliveryCopy = null;

        await SetBusyAsync(false);
    }
}