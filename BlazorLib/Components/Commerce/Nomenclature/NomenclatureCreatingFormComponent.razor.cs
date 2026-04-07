////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Commerce.Nomenclature;

/// <summary>
/// NomenclatureCreatingFormComponent
/// </summary>
public partial class NomenclatureCreatingFormComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// NomenclatureCreatingHandler
    /// </summary>
    [Parameter, EditorRequired]
    public required Action<NomenclatureModelDB> NomenclatureCreatingHandler { get; set; }

    /// <summary>
    /// ContextName
    /// </summary>
    [Parameter]
    public string? ContextName { get; set; }


    UnitsOfMeasurementEnum UMeas { get; set; } = UnitsOfMeasurementEnum.Thing;

    string? CreatingNomenclatureName { get; set; }

    bool CanSave => !string.IsNullOrWhiteSpace(CreatingNomenclatureName);

    async Task CreateNewNomenclature()
    {
        if (string.IsNullOrWhiteSpace(CreatingNomenclatureName))
            return;

        NomenclatureModelDB new_obj = new()
        {
            Name = CreatingNomenclatureName,
            BaseUnit = UMeas,
            ContextName = ContextName,
            IsDisabled = true,
        };

        await SetBusyAsync();

        TResponseModel<int> res = await CommerceRepo.NomenclatureUpdateOrCreateAsync(new_obj);
        
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success() && res.Response > 0)
        {
            new_obj.Id = res.Response;
            NomenclatureCreatingHandler(new_obj);

            CreatingNomenclatureName = null;
            UMeas = UnitsOfMeasurementEnum.Thing;
        }
        await SetBusyAsync(false);
    }
}