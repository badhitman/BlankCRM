////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// OfferCreatingFormComponent
/// </summary>
public partial class OfferCreatingFormComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


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

    bool CanSave => priceOffer > 0 && multiplicityOffer > 0 && UnitOffer != UnitsOfMeasurementEnum.None;

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
        };
        await SetBusyAsync();
        
        TResponseModel<int> res = await CommerceRepo.OfferUpdateAsync(new() { Payload = off, SenderActionUserId = CurrentUserSession.UserId });

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response > 0)
        {
            off.Id = res.Response;
            OfferCreatingHandler(off);

            UnitOffer = UnitsOfMeasurementEnum.None;
            priceOffer = 0;
            multiplicityOffer = 0;
            nameOffer = null;
        }
        await SetBusyAsync(false);
    }
}