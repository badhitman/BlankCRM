////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.Helpdesk;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// OfficeOrganizationComponent
/// </summary>
public partial class OfficeOrganizationComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IHelpdeskTransmission HelpdeskRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// AddressForOrganization
    /// </summary>
    [Parameter, EditorRequired]
    public required int AddressForOrganization { get; set; }


    OfficeOrganizationModelDB OfficeCurrent { get; set; } = default!;
    OfficeOrganizationModelDB OfficeEdit { get; set; } = default!;

    bool CanSave =>
        (OfficeEdit.Address != OfficeCurrent.Address ||
        OfficeEdit.Contacts != OfficeCurrent.Contacts ||
        OfficeEdit.Name != OfficeCurrent.Name ||
        OfficeEdit.ParentId != OfficeCurrent.ParentId) &&
        SelectedRubric is not null;

    UniversalBaseModel? SelectedRubric;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();

        TResponseModel<OfficeOrganizationModelDB[]> res_address = await CommerceRepo
            .OfficesOrganizationsRead([AddressForOrganization]);

        SnackbarRepo.ShowMessagesResponse(res_address.Messages);
        OfficeCurrent = res_address.Response!.Single();
        OfficeEdit = GlobalTools.CreateDeepCopy(OfficeCurrent) ?? throw new Exception();

        TResponseModel<List<RubricIssueHelpdeskModelDB>> res_rubric = await HelpdeskRepo.RubricRead(OfficeCurrent.ParentId);
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
        OfficeEdit = GlobalTools.CreateDeepCopy(OfficeCurrent) ?? throw new Exception();
        if (rubricSelector_ref.SelectedRubricId != OfficeEdit.ParentId)
        {
            rubricSelector_ref.SelectedRubricId = OfficeEdit.ParentId;
            rubricSelector_ref.StateHasChangedCall();
        }
    }

    async Task SaveOffice()
    {
        if (!CanSave)
            return;

        await SetBusy();

        TResponseModel<int> res = await CommerceRepo.OfficeOrganizationUpdate(new AddressOrganizationBaseModel()
        {
            Address = OfficeEdit.Address!,
            Name = OfficeEdit.Name!,
            ParentId = SelectedRubric!.Id,
            Contacts = OfficeEdit.Contacts,
            Id = AddressForOrganization,
        });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        if (!res.Success())
            return;

        OfficeCurrent = GlobalTools.CreateDeepCopy(OfficeEdit) ?? throw new Exception();
    }

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        SelectedRubric = selectedRubric;
        OfficeEdit.ParentId = selectedRubric?.Id ?? 0;
        StateHasChanged();
    }
}