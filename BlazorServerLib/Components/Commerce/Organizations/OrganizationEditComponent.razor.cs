﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// OrganizationEditComponent
/// </summary>
public partial class OrganizationEditComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// OrganizationId
    /// </summary>
    [Parameter]
    public int? OrganizationId { get; set; }

    OrganizationModelDB? currentOrg;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        OrganizationId ??= 0;
        await ReadCurrentUser();

        if (OrganizationId == 0)
        {
            currentOrg = new()
            {
                Email = string.Empty,
                INN = string.Empty,
                KPP = string.Empty,
                LegalAddress = string.Empty,
                Name = string.Empty,
                OGRN = string.Empty,
                Phone = string.Empty,
            };

            return;
        }

        await ReadOrganization();
    }

    async Task ReadOrganization()
    {
        if (OrganizationId is null)
            return;

        await SetBusyAsync();
        TResponseModel<OrganizationModelDB[]> res = await CommerceRepo.OrganizationsReadAsync([OrganizationId.Value]);
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        currentOrg = res.Response!.Single();
        if (currentOrg is not null && (currentOrg.Users?.Any(x => x.UserPersonIdentityId == CurrentUserSession!.UserId) != true && !CurrentUserSession!.IsAdmin && CurrentUserSession!.Roles?.Any(x => GlobalStaticConstants.Roles.AllHelpDeskRoles.Contains(x)) != true))
        {
            currentOrg = null;
            return;
        }
    }
}