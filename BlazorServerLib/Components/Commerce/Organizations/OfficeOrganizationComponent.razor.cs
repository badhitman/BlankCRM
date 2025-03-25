////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.Helpdesk;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        (OfficeEdit.AddressUserComment != OfficeCurrent.AddressUserComment ||
        OfficeEdit.Contacts != OfficeCurrent.Contacts ||
        OfficeEdit.Name != OfficeCurrent.Name ||
        OfficeEdit.KladrCode != OfficeCurrent.KladrCode ||
        OfficeEdit.KladrTitle != OfficeCurrent.KladrTitle ||
        OfficeEdit.ParentId != OfficeCurrent.ParentId) &&
        SelectedRubric is not null;

    UniversalBaseModel? SelectedRubric;

    void ChangeSelectAction(KladrResponseModel sender)
    {
        OfficeEdit.KladrCode = sender.Code;
        OfficeEdit.KladrTitle = sender.Name;
        StateHasChangedCall();         
    }

    void HandleOnChange(ChangeEventArgs args)
    {
        OfficeEdit.AddressUserComment = args.Value?.ToString() ?? "";

        //await ChildDataChanged.InvokeAsync(data);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusy();

        TResponseModel<OfficeOrganizationModelDB[]> res_address = await CommerceRepo
            .OfficesOrganizationsReadAsync([AddressForOrganization]);

        SnackbarRepo.ShowMessagesResponse(res_address.Messages);
        OfficeCurrent = res_address.Response!.Single();
        OfficeEdit = GlobalTools.CreateDeepCopy(OfficeCurrent) ?? throw new Exception();

        TResponseModel<List<RubricIssueHelpdeskModelDB>> res_rubric = await HelpdeskRepo.RubricReadAsync(OfficeCurrent.ParentId);
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

        TResponseModel<int> res = await CommerceRepo.OfficeOrganizationUpdateAsync(new AddressOrganizationBaseModel()
        {
            KladrCode = OfficeEdit.KladrCode,
            KladrTitle = OfficeEdit.KladrTitle,
            AddressUserComment = OfficeEdit.AddressUserComment!,
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