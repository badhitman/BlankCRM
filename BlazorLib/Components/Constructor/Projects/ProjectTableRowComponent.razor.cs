﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor.Projects;

/// <summary>
/// Строка таблицы проектов
/// </summary>
public partial class ProjectTableRowComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IDialogService DialogService { get; set; } = default!;

    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;


    /// <summary>
    /// Проект
    /// </summary>
    [Parameter, EditorRequired]
    public required ProjectViewModel ProjectRow { get; set; }

    /// <summary>
    /// Ссылка на 
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required ProjectsListComponent ParentProjectsList { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    bool IsMyProject => CurrentUserSession!.UserId.Equals(ProjectRow.OwnerUserId);

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
    }

    /// <inheritdoc/>
    protected async Task EditProject()
    {
        DialogParameters<ProjectEditDialogComponent> parameters = new()
        {
             { x => x.ProjectForEdit, ProjectRow },
             { x => x.ParentFormsPage, ParentFormsPage },
             { x => x.ParentListProjects, ParentProjectsList },
             { x => x.CurrentUser, CurrentUserSession ! },
        };
        DialogOptions options = new() { CloseButton = true, MaxWidth = MaxWidth.ExtraExtraLarge };
        IDialogReference res = await DialogService.ShowAsync<ProjectEditDialogComponent>("Редактирование проекта", parameters, options);
        await ParentProjectsList.ReloadListProjects();
    }

    /// <inheritdoc/>
    protected async Task DeleteProject()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await ConstructorRepo.SetMarkerDeleteProjectAsync(new() { ProjectId = ProjectRow.Id, Marker = !ProjectRow.IsDisabled });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ParentProjectsList.ReloadListProjects();
        ParentProjectsList.StateHasChangedCall();
    }

    /// <inheritdoc/>
    protected async Task SetMainProjectHandle()
    {
        await SetBusyAsync();

        ResponseBaseModel res = await ConstructorRepo.SetProjectAsMainAsync(new() { ProjectId = ProjectRow.Id, UserId = CurrentUserSession!.UserId });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success())
        {
            await ParentFormsPage.ReadCurrentMainProject();
            ParentFormsPage.StateHasChangedCall();
        }
    }
}