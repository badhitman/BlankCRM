////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Rubrics.Properties;

/// <inheritdoc/>
public partial class RubricFieldsManageComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int ProjectId { get; set; }


    static TypesFieldsFormsEnum[] simpleTypesFields = [TypesFieldsFormsEnum.Bool, TypesFieldsFormsEnum.Int];
    List<FormConstructorModelDB> allForms = [];
    FormConstructorModelDB? Form { get; set; }

    int _selectedFormId;
    int SelectedFormId
    {
        get => _selectedFormId;
        set
        {
            _selectedFormId = value;
            Form = allForms.First(x => x.Id == value);
        }
    }

    void ReloadHandleAction()
    {
        
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
            return;

        TPaginationResponseStandardModel<FormConstructorModelDB> res = await ConstructorRepo
            .SelectFormsAsync(new()
            {
                ProjectId = ProjectId,
                Request = new()
                {
                    PageSize = int.MaxValue
                }
            });

        allForms = res.Response ?? [];
        if (res.Response is not null && res.Response.Count == 0)
        {
            TResponseModel<FormConstructorModelDB> initForm = await ConstructorRepo.FormUpdateOrCreateAsync(new()
            {
                SenderActionUserId = CurrentUserSession.UserId,
                Payload = new()
                {
                    ProjectId = ProjectId,
                    Name = "default",
                }
            });
            SnackBarRepo.ShowMessagesResponse(initForm.Messages);
            if (initForm.Response is not null)
                allForms = [initForm.Response];
        }

        Form = allForms.FirstOrDefault();

        await SetBusyAsync(false);
    }
}