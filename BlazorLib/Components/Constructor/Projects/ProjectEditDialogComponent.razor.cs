﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;
using static SharedLib.GlobalStaticConstants;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Constructor.Projects;

/// <summary>
/// ProjectEditDialogComponent
/// </summary>
public partial class ProjectEditDialogComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ProjectViewModel ProjectForEdit { get; set; }
    ProjectViewModel projectObject = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ProjectsListComponent ParentListProjects { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required UserInfoMainModel CurrentUser { get; set; }


    string images_upload_url = default!;
    Dictionary<string, object> editorConf = default!;


    /// <inheritdoc/>
    protected bool CanSave => !string.IsNullOrWhiteSpace(projectObject.Name) && (!ProjectForEdit.Equals(projectObject) || ProjectForEdit.Id < 1);

    void ResetForm()
    {
        if (projectObject is null)
            projectObject = ProjectViewModel.Build(ProjectForEdit);
        else
            projectObject.Reload(ProjectForEdit);

        _currentTemplateInputRichText?.SetValue(projectObject.Description);
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        ResetForm();
        images_upload_url = $"{TinyMCEditorUploadImage}{Routes.PROJECTS_CONTROLLER_NAME}/{Routes.CONFIGURATION_CONTROLLER_NAME}?{nameof(StorageMetadataModel.PrefixPropertyName)}={Routes.DESCRIPTION_CONTROLLER_NAME}&{nameof(StorageMetadataModel.OwnerPrimaryKey)}={projectObject.Id}";
        editorConf = TinyMCEditorConf(images_upload_url);
    }

    /// <inheritdoc/>
    protected InputRichTextComponent? _currentTemplateInputRichText;

    /// <inheritdoc/>
    protected async Task SaveProject()
    {
        await SetBusyAsync();
        if (projectObject.Id < 1)
        {
            TResponseModel<int> res = await ConstructorRepo.CreateProjectAsync(new() { Project = projectObject, UserId = CurrentUser.UserId });
            IsBusyProgress = false;
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
            {
                ProjectForEdit.Id = res.Response;
                projectObject.Reload(ProjectForEdit);
                await ParentListProjects.ReloadListProjects();
                ParentListProjects.StateHasChangedCall();
            }
        }
        else
        {
            ResponseBaseModel res = await ConstructorRepo.UpdateProjectAsync(projectObject);
            IsBusyProgress = false;
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Success())
            {
                ProjectForEdit.Reload(projectObject);
                await ParentListProjects.ReloadListProjects();
                ParentListProjects.StateHasChangedCall();
            }
        }
        // MudDialog.Close(DialogResult.Ok(true));
    }

    /// <inheritdoc/>
    protected void Cancel() => MudDialog.Cancel();
}