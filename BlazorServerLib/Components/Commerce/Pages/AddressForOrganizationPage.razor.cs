﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;
using BlazorWebLib.Components.Helpdesk;

namespace BlazorWebLib.Components.Commerce.Pages;

/// <summary>
/// AddressForOrganizationPage
/// </summary>
public partial class AddressForOrganizationPage : BlazorBusyComponentBaseModel
{
    [Inject]
    IHelpdeskTransmission HelpdeskRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// AddressForOrganization
    /// </summary>
    [Parameter]
    public int AddressForOrganization { get; set; } = default!;


    AddressOrganizationModelDB AddressCurrent { get; set; } = default!;
    AddressOrganizationModelDB AddressEdit { get; set; } = default!;

    bool CanSave =>
        (AddressEdit.Address != AddressCurrent.Address ||
        AddressEdit.Contacts != AddressCurrent.Contacts ||
        AddressEdit.Name != AddressCurrent.Name ||
        AddressEdit.ParentId != AddressCurrent.ParentId) &&
        SelectedRubric is not null;

    UniversalBaseModel? SelectedRubric;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();

        TResponseModel<AddressOrganizationModelDB[]> res_address = await CommerceRepo
            .AddressesOrganizationsRead([AddressForOrganization]);

        SnackbarRepo.ShowMessagesResponse(res_address.Messages);
        AddressCurrent = res_address.Response!.Single();
        AddressEdit = GlobalTools.CreateDeepCopy(AddressCurrent) ?? throw new Exception();

        TResponseModel<List<RubricIssueHelpdeskModelDB>> res_rubric = await HelpdeskRepo.RubricRead(AddressCurrent.ParentId);
        await SetBusy(false);
        SnackbarRepo.ShowMessagesResponse(res_rubric.Messages);
        if (res_rubric.Success() && res_rubric.Response is not null && res_rubric.Response.Count != 0)
        {
            RubricIssueHelpdeskModelDB r = res_rubric.Response.First();
            SelectedRubric = new UniversalBaseModel()
            {
                Name = r.Name,
                Description = r.Description,
                Id = r.Id,
                IsDisabled = r.IsDisabled,
                ParentId = r.ParentId,
                ProjectId = r.ProjectId,
                SortIndex = r.SortIndex,
            };
        }
    }

    RubricSelectorComponent rubricSelector_ref = default!;

    void ResetEdit()
    {
        AddressEdit = GlobalTools.CreateDeepCopy(AddressCurrent) ?? throw new Exception();
        if (rubricSelector_ref.SelectedRubricId != AddressEdit.ParentId)
        {
            rubricSelector_ref.SelectedRubricId = AddressEdit.ParentId;
            rubricSelector_ref.StateHasChangedCall();
        }
    }

    async Task SaveAddress()
    {
        if (!CanSave)
            return;

        await SetBusy();

        TResponseModel<int> res = await CommerceRepo.AddressOrganizationUpdate(new AddressOrganizationBaseModel()
        {
            Address = AddressEdit.Address!,
            Name = AddressEdit.Name!,
            ParentId = SelectedRubric!.Id,
            Contacts = AddressEdit.Contacts,
            Id = AddressForOrganization,
        });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (!res.Success())
            return;

        AddressCurrent = GlobalTools.CreateDeepCopy(AddressEdit) ?? throw new Exception();
    }

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        SelectedRubric = selectedRubric;
        AddressEdit.ParentId = selectedRubric?.Id ?? 0;
        StateHasChanged();
    }
}