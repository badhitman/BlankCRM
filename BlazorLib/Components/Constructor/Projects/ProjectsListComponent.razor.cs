////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib.Components.Constructor;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorLib.Components.Constructor.Projects;

/// <inheritdoc/>
public partial class ProjectsListComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;

    [Inject]
    IDialogService DialogService { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required ConstructorMainManageComponent ParentFormsPage { get; set; }


    /// <summary>
    /// Проекты пользователя
    /// </summary>
    public ProjectViewModel[]? ProjectsOfUser { get; private set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadListProjects();
    }

    /// <summary>
    /// Загрузка перечня проектов
    /// </summary>
    public async Task ReloadListProjects()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();

        TResponseModel<ProjectViewModel[]> res_pr = await ConstructorRepo.GetProjectsForUserAsync(new() { UserId = CurrentUserSession.UserId });
        ProjectsOfUser = res_pr.Response ?? throw new Exception();
        await SetBusyAsync(false);
    }

    /// <summary>
    /// Создать проект
    /// </summary>
    protected async Task CreateProject()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (ProjectsOfUser is null)
            throw new Exception("ProjectsOfUser is null");

        int i = 1;
        string name_new_project = $"Новый проект {i}";
        while (ProjectsOfUser.Any(x => x.Name.Equals(name_new_project, StringComparison.OrdinalIgnoreCase)))
        {
            i++;
            name_new_project = $"Новый проект {i}";
        }

        DialogParameters<ProjectEditDialogComponent> parameters = new()
        {
             { x => x.ProjectForEdit, new ProjectViewModel() { Name = name_new_project, OwnerUserId = CurrentUserSession.UserId } },
             { x => x.ParentFormsPage, ParentFormsPage },
             { x => x.ParentListProjects, this },
             { x => x.CurrentUser, CurrentUserSession },
        };
        DialogOptions options = new() { CloseButton = true, MaxWidth = MaxWidth.ExtraExtraLarge };
        IDialogReference res = await DialogService.ShowAsync<ProjectEditDialogComponent>("Создание проекта", parameters, options);

        await ReloadListProjects();
    }
}