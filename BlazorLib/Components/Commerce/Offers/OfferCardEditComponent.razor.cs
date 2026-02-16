////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Commerce.Offers;

/// <summary>
/// OfferCardEditComponent
/// </summary>
public partial class OfferCardEditComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;


    /// <summary>
    /// OfferId
    /// </summary>
    [Parameter, EditorRequired]
    public int OfferId { get; set; }


    OfferModelDB? CurrentOffer, editOffer;
    FilesContextViewComponent? filesViewRef;
    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;
    bool allowOfferFreePrice;

    bool CanSave => CurrentOffer is null || editOffer is null ||
        editOffer.IsDisabled != CurrentOffer.IsDisabled ||
        editOffer.ShortName != CurrentOffer.ShortName ||
        editOffer.Name != CurrentOffer.Name ||
        editOffer.QuantitiesTemplate != CurrentOffer.QuantitiesTemplate ||
        (editOffer.Price != CurrentOffer.Price && ((allowOfferFreePrice && editOffer.Price == 0) || editOffer.Price > 0)) ||
        editOffer.Multiplicity != CurrentOffer.Multiplicity ||
        editOffer.OfferUnit != CurrentOffer.OfferUnit ||
        editOffer.Weight != CurrentOffer.Weight;

    async Task SaveOffer()
    {
        if (CurrentUserSession is null)
            return;

        await SetBusyAsync();
        TResponseModel<int> res = await CommerceRepo.OfferUpdateOrCreateAsync(new() { Payload = editOffer, SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response > 0)
            CurrentOffer = GlobalTools.CreateDeepCopy(editOffer)!;

        if (filesViewRef is not null)
            await filesViewRef.ReloadServerData();
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.OFFER_CONTROLLER_NAME}/{Routes.BODY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={OfferId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        await SetBusyAsync();
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        List<Task> tasks = [
               Task.Run(async () => { TResponseModel<bool?> res = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.AllowOfferFreePrice); allowOfferFreePrice = res.Response == true; }),
                Task.Run(async () => {
                    TResponseModel<OfferModelDB[]> res = await CommerceRepo.OffersReadAsync(new() { Payload = [OfferId], SenderActionUserId = CurrentUserSession.UserId });
                    SnackBarRepo.ShowMessagesResponse(res.Messages);
                    CurrentOffer = res.Response!.Single();
                })
           ];
        await Task.WhenAll(tasks);

        editOffer = GlobalTools.CreateDeepCopy(CurrentOffer)!;
        await SetBusyAsync(false);
    }
}