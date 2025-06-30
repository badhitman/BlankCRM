////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using BlazorLib.Components.Rubrics;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// OfficeOrganizationComponent
/// </summary>
public partial class OfficeOrganizationComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// AddressForOrganization
    /// </summary>
    [Parameter, EditorRequired]
    public required int AddressForOrganization { get; set; }


    OfficeOrganizationModelDB? OfficeCurrent { get; set; }
    OfficeOrganizationModelDB? OfficeEdit { get; set; }

    bool CanSave =>
        (OfficeEdit?.AddressUserComment != OfficeCurrent?.AddressUserComment ||
        OfficeEdit?.Contacts != OfficeCurrent?.Contacts ||
        OfficeEdit?.Name != OfficeCurrent?.Name ||
        OfficeEdit?.KladrCode != OfficeCurrent?.KladrCode ||
        OfficeEdit?.KladrTitle != OfficeCurrent?.KladrTitle ||
        OfficeEdit?.ParentId != OfficeCurrent?.ParentId);

    UniversalBaseModel? SelectedRubric;

    EntryAltModel? SelectedKladrObject
    {
        get => EntryAltModel.Build(OfficeEdit!.KladrCode, OfficeEdit.KladrTitle);
        set
        {
            OfficeEdit!.KladrCode = value?.Id ?? "";
            OfficeEdit.KladrTitle = value?.Name ?? "";
        }
    }


    void HandleOnChange(ChangeEventArgs args)
    {
        OfficeEdit!.AddressUserComment = args.Value?.ToString() ?? "";

        //await ChildDataChanged.InvokeAsync(data);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();

        TResponseModel<OfficeOrganizationModelDB[]> res_address = await CommerceRepo
            .OfficesOrganizationsReadAsync([AddressForOrganization]);

        SnackBarRepo.ShowMessagesResponse(res_address.Messages);
        OfficeCurrent = res_address.Response!.Single();
        OfficeEdit = GlobalTools.CreateDeepCopy(OfficeCurrent) ?? throw new Exception();

        TResponseModel<List<RubricStandardModel>> res_rubric = await HelpDeskRepo.RubricReadAsync(OfficeCurrent.ParentId);
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res_rubric.Messages);
        if (res_rubric.Success() && res_rubric.Response is not null && res_rubric.Response.Count != 0)
        {
            RubricStandardModel r = res_rubric.Response.First();
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

        await SetBusyAsync();

        TResponseModel<int> res = await CommerceRepo.OfficeOrganizationUpdateAsync(new AddressOrganizationBaseModel()
        {
            KladrCode = OfficeEdit!.KladrCode,
            KladrTitle = OfficeEdit.KladrTitle,
            AddressUserComment = OfficeEdit.AddressUserComment!,
            Name = OfficeEdit.Name!,
            ParentId = SelectedRubric?.Id ?? 0,
            Contacts = OfficeEdit.Contacts,
            Id = AddressForOrganization,
        });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (!res.Success())
            return;

        OfficeCurrent = GlobalTools.CreateDeepCopy(OfficeEdit) ?? throw new Exception();
    }

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        SelectedRubric = selectedRubric;
        OfficeEdit!.ParentId = selectedRubric?.Id ?? 0;
        StateHasChanged();
    }
}