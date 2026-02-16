////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce.Offers;

/// <summary>
/// OfferCreatingFormComponent
/// </summary>
public partial class OfferCreatingFormComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission StorageTransmissionRepo { get; set; } = default!;


    /// <summary>
    /// Current Nomenclature
    /// </summary>
    [Parameter, EditorRequired]
    public required NomenclatureModelDB CurrentNomenclature { get; set; }

    /// <summary>
    /// OfferCreatingHandler
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<OfferModelDB> OfferCreatingHandler { get; set; }


    UnitsOfMeasurementEnum UnitOffer { get; set; } = UnitsOfMeasurementEnum.None;
    decimal priceOffer;
    uint multiplicityOffer;
    string? nameOffer;
    decimal weightOffer;
    bool allowOfferFreePrice;

    bool CanSave =>
        (priceOffer > 0 || (allowOfferFreePrice && priceOffer == 0)) &&
        multiplicityOffer > 0 &&
        UnitOffer != UnitsOfMeasurementEnum.None;

    async Task AddOffer()
    {
        if (CurrentUserSession is null)
            return;

        OfferModelDB off = new()
        {
            Name = nameOffer ?? "",
            NomenclatureId = CurrentNomenclature.Id,
            Multiplicity = multiplicityOffer,
            OfferUnit = UnitOffer,
            Price = priceOffer,
            Weight = weightOffer,
        };
        await SetBusyAsync();

        TResponseModel<int> res = await CommerceRepo.OfferUpdateOrCreateAsync(new()
        {
            Payload = off,
            SenderActionUserId = CurrentUserSession.UserId
        });

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response > 0)
        {
            off.Id = res.Response;
            OfferCreatingHandler(off);

            UnitOffer = UnitsOfMeasurementEnum.None;
            priceOffer = 0;
            multiplicityOffer = 0;
            nameOffer = null;
            weightOffer = 0;
        }
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        List<Task> tasks = [
                Task.Run(async () => { TResponseModel<bool?> res = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.AllowOfferFreePrice); allowOfferFreePrice = res.Response == true; }),
                Task.Run(base.OnInitializedAsync)
            ];
        await Task.WhenAll(tasks);
        await SetBusyAsync(false);
    }
}