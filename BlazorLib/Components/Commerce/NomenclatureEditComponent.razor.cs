////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using System.Reflection;
using BlazorLib.Components.Commerce.Attendances;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Commerce;

/// <summary>
/// NomenclatureEditComponent
/// </summary>
public partial class NomenclatureEditComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// Nomenclature
    /// </summary>
    [Parameter, EditorRequired]
    public required int NomenclatureId { get; set; }

    /// <summary>
    /// ModeView
    /// </summary>
    [CascadingParameter, EditorRequired]
    public OffersListModesEnum ViewSet { get; set; } = OffersListModesEnum.Goods;


    NomenclatureModelDB? CurrentNomenclature;
    NomenclatureModelDB? editNomenclature;
    FilesContextViewComponent? filesViewRef;

    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;

    bool CanSave => 
        CurrentNomenclature is not null &&
        (CurrentNomenclature.IsDisabled != editNomenclature?.IsDisabled ||
        CurrentNomenclature.Name != editNomenclature?.Name || 
        CurrentNomenclature.Description != editNomenclature?.Description || 
        CurrentNomenclature.BaseUnit != editNomenclature?.BaseUnit);

    static Type? GetType(string strFullyQualifiedName)
    {
        return Assembly.GetExecutingAssembly().GetTypes().Single(t => t.Name == strFullyQualifiedName);
    }

    Dictionary<string, object> Parameters
    {
        get
        {
            Dictionary<string, object> par = [];
            par.Add(nameof(OffersAttendancesListComponent.CurrentNomenclature), editNomenclature!);
            //par.Add(nameof(OfferBalanceBaseModel.Parent), this);
            return par;
        }
    }


    async Task SaveNomenclature()
    {
        if (editNomenclature is null)
            throw new ArgumentNullException(nameof(editNomenclature));

        await SetBusyAsync();

        TResponseModel<int> res = await CommerceRepo.NomenclatureUpdateAsync(editNomenclature);
        
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success())
            CurrentNomenclature = GlobalTools.CreateDeepCopy(editNomenclature);

        if (filesViewRef is not null)
            await filesViewRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        images_upload_url = $"{GlobalStaticConstants.TinyMCEditorUploadImage}{Routes.NOMENCLATURE_CONTROLLER_NAME}/{Routes.BODY_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.IMAGE_ACTION_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={NomenclatureId}";
        editorConf = GlobalStaticConstants.TinyMCEditorConf(images_upload_url);

        await base.OnInitializedAsync();
        await SetBusyAsync();
        if (CurrentUserSession is null)
            throw new Exception();

        TResponseModel<List<NomenclatureModelDB>> res = await CommerceRepo.NomenclaturesReadAsync(new() { Payload = [NomenclatureId], SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (res.Response is not null && res.Response.Count != 0)
        {
            CurrentNomenclature = res.Response.Single();
            editNomenclature = GlobalTools.CreateDeepCopy(CurrentNomenclature);
        }
        await SetBusyAsync(false);
    }
}