﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Net.Mail;

namespace BlazorLib.Components.Constructor.Projects;

/// <summary>
/// MembersOfProject
/// </summary>
public partial class MembersOfProjectComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IConstructorTransmission ConstructorRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// Project Id
    /// </summary>
    [Parameter, EditorRequired]
    public required ProjectViewModel ProjectView { get; set; }


    /// <summary>
    /// Ссылка на 
    /// </summary>
    [Parameter, EditorRequired]
    public required ProjectsListComponent ProjectsList { get; set; }


    string? emailForAddMember;

    /// <inheritdoc/>
    public async Task AddMember()
    {
        if (!MailAddress.TryCreate(emailForAddMember, out _))
            throw new Exception($"Email не корректный '{emailForAddMember}'");

        await SetBusyAsync();
        TResponseModel<UserInfoModel>? user_info = await IdentityRepo.FindUserByEmailAsync(emailForAddMember);

        if (user_info.Response is null)
            SnackBarRepo.Error($"Пользователь с Email '{emailForAddMember}' не найден");
        else
        {
            ResponseBaseModel adding_member = await ConstructorRepo.AddMembersToProjectAsync(new() { ProjectId = ProjectView.Id, UsersIds = [user_info.Response.UserId] });
            SnackBarRepo.ShowMessagesResponse(adding_member.Messages);
        }
        IsBusyProgress = false;
        emailForAddMember = null;
        TResponseModel<EntryAltModel[]> members_rest = await ConstructorRepo.GetMembersOfProjectAsync(ProjectView.Id);
        ProjectView.Members = new(members_rest.Response ?? throw new Exception());

        await ProjectsList.ReloadListProjects();
        ProjectsList.StateHasChangedCall();
    }

    async Task Closed(MudChip<EntryAltModel> chip)
    {
        if (chip.Value is null)
            throw new Exception();

        await SetBusyAsync();
        ResponseBaseModel res = await ConstructorRepo.DeleteMembersFromProjectAsync(new() { ProjectId = ProjectView.Id, UsersIds = [chip.Value.Id] });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        TResponseModel<EntryAltModel[]> rest_members = await ConstructorRepo.GetMembersOfProjectAsync(ProjectView.Id);
        ProjectView.Members = new(rest_members.Response ?? throw new Exception());

        await ProjectsList.ReloadListProjects();
        ProjectsList.StateHasChangedCall();
    }
}