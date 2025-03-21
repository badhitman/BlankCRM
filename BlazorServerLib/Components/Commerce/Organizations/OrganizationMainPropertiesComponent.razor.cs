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

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        editOrg = GlobalTools.CreateDeepCopy(CurrentOrganization);
    }

    async Task ReadOrganization()
    {
        if (CurrentOrganization is null)
            return;

        await SetBusy();
        TResponseModel<OrganizationModelDB[]> res = await CommerceRepo.OrganizationsRead([CurrentOrganization.Id]);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        CurrentOrganization = res.Response!.Single();
        if (CurrentOrganization is not null && (CurrentOrganization.Users?.Any(x => x.UserPersonIdentityId == CurrentUserSession!.UserId) != true && !CurrentUserSession!.IsAdmin && CurrentUserSession!.Roles?.Any(x => GlobalStaticConstants.Roles.AllHelpDeskRoles.Contains(x)) != true))
            return;
    }

    async Task CancelEditRequestOrganization()
    {
        if (editOrg is null || editOrg.Equals(CurrentOrganization))
            return;

        editOrg = GlobalTools.CreateDeepCopy(CurrentOrganization);

        TAuthRequestModel<OrganizationModelDB> req = new() { Payload = editOrg!, SenderActionUserId = CurrentUserSession!.UserId };
        await SetBusy();

        TResponseModel<int> res = await CommerceRepo.OrganizationUpdate(req);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
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

        await SetBusy();

        TResponseModel<bool> res = await CommerceRepo.OrganizationSetLegal(req);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        NavigationRepo.ReloadPage();
    }

    async Task SaveOrganization()
    {
        if (editOrg is null || editOrg.Equals(CurrentOrganization))
            throw new ArgumentNullException(nameof(editOrg));

        TAuthRequestModel<OrganizationModelDB> req = new() { Payload = editOrg!, SenderActionUserId = CurrentUserSession!.UserId };
        await SetBusy();

        TResponseModel<int> res = await CommerceRepo.OrganizationUpdate(req);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);

        if (CurrentOrganization.Id == 0)
        {
            NavigationRepo.NavigateTo($"/organizations/edit/{res.Response}");
            return;
        }
        else
            await ReadOrganization();
    }
}