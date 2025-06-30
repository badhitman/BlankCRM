﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// OrganizationMainPropertiesComponent
/// </summary>
public partial class OrganizationMainPropertiesComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    NavigationManager NavigationRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required OrganizationModelDB CurrentOrganization { get; set; }


    OrganizationModelDB? editOrg;
    BanksListDetailsOrganizationComponent? banksListDetailsRef;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        editOrg = GlobalTools.CreateDeepCopy(CurrentOrganization);
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender && banksListDetailsRef?.PanelsRef is not null && banksListDetailsRef.IsExpanded && !CurrentOrganization.Equals(editOrg))
        {
            await banksListDetailsRef.PanelsRef.CollapseAllAsync();
            banksListDetailsRef.StateHasChangedCall();
        }
    }

    async Task ReadOrganization()
    {
        if (CurrentOrganization is null)
            return;

        await SetBusyAsync();
        TResponseModel<OrganizationModelDB[]> res = await CommerceRepo.OrganizationsReadAsync([CurrentOrganization.Id]);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        CurrentOrganization = res.Response!.Single();
        if (CurrentOrganization is not null && (CurrentOrganization.Users?.Any(x => x.UserPersonIdentityId == CurrentUserSession!.UserId) != true && !CurrentUserSession!.IsAdmin && CurrentUserSession!.Roles?.Any(x => GlobalStaticConstantsRoles.Roles.AllHelpDeskRoles.Contains(x)) != true))
            return;
    }

    async Task CancelEditRequestOrganization()
    {
        if (editOrg is null || editOrg.Equals(CurrentOrganization))
            return;

        editOrg = GlobalTools.CreateDeepCopy(CurrentOrganization);

        TAuthRequestModel<OrganizationModelDB> req = new() { Payload = editOrg!, SenderActionUserId = CurrentUserSession!.UserId };
        await SetBusyAsync();

        TResponseModel<int> res = await CommerceRepo.OrganizationUpdateAsync(req);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReadOrganization();
    }

    async Task ConfirmChangeOrganization()
    {
        if (editOrg is null)
            throw new ArgumentNullException(nameof(editOrg));

        OrganizationModelDB req = GlobalTools.CreateDeepCopy(editOrg)!;

        if (!string.IsNullOrWhiteSpace(editOrg.NewINN))
            req.INN = editOrg.NewINN;

        if (!string.IsNullOrWhiteSpace(editOrg.NewOGRN))
            req.OGRN = editOrg.NewOGRN;

        if (!string.IsNullOrWhiteSpace(editOrg.NewLegalAddress))
            req.LegalAddress = editOrg.NewLegalAddress;

        if (!string.IsNullOrWhiteSpace(editOrg.NewKPP))
            req.KPP = editOrg.NewKPP;

        if (!string.IsNullOrWhiteSpace(editOrg.NewName))
            req.Name = editOrg.NewName;

        await SetBusyAsync();

        TResponseModel<bool> res = await CommerceRepo.OrganizationSetLegalAsync(req);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        NavigationRepo.ReloadPage();
    }

    async Task SaveOrganization()
    {
        if (editOrg is null || editOrg.Equals(CurrentOrganization))
            throw new ArgumentNullException(nameof(editOrg));

        TAuthRequestModel<OrganizationModelDB> req = new() { Payload = editOrg!, SenderActionUserId = CurrentUserSession!.UserId };
        await SetBusyAsync();

        TResponseModel<int> res = await CommerceRepo.OrganizationUpdateAsync(req);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (CurrentOrganization.Id == 0)
        {
            NavigationRepo.NavigateTo($"/organizations/edit/{res.Response}");
            return;
        }
        else
            await ReadOrganization();
    }
}