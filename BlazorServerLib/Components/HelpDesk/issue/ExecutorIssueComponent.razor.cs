////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;
using System.Net.Mail;

namespace BlazorWebLib.Components.HelpDesk.issue;

/// <summary>
/// ExecutorIssueComponent
/// </summary>
public partial class ExecutorIssueComponent : IssueWrapBaseModel
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;

    UserInfoModel? Executor;
    string? editExecutorEmail;
    bool IsEditMode;

    async Task SetMeAsExecutor()
    {
        await SetExecutor(CurrentUserSession!.UserId);
    }

    async Task SetNewExecutor()
    {
        if (!string.IsNullOrWhiteSpace(editExecutorEmail) && !MailAddress.TryCreate(editExecutorEmail, out _))
            throw new Exception();

        UserInfoModel? user_by_email = null;
        if (!string.IsNullOrWhiteSpace(editExecutorEmail))
        {
            await SetBusyAsync();
            TResponseModel<UserInfoModel[]> res = await IdentityRepo.GetUsersIdentityByEmailsAsync([editExecutorEmail]);
            user_by_email = res.Response?.FirstOrDefault();
            IsBusyProgress = false;
            if (user_by_email is null)
            {
                SnackbarRepo.Error($"User with this email not found: {editExecutorEmail}");
                return;
            }

            if (user_by_email.Roles?.Any(x => GlobalStaticConstantsRoles.Roles.AllHelpDeskRoles.Contains(x)) != true && !user_by_email.IsAdmin)
            {
                SnackbarRepo.Error($"User {editExecutorEmail} cannot be installed by the performer: is not an employee");
                return;
            }
        }

        await SetExecutor(user_by_email?.UserId ?? "");
    }

    async Task SetExecutor(string user_id)
    {
        await SetBusyAsync();
        TResponseModel<bool> rest = await HelpDeskRepo.ExecuterUpdateAsync(new() { SenderActionUserId = CurrentUserSession!.UserId, Payload = new() { IssueId = Issue.Id, UserId = user_id } });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(rest.Messages);

        if (!rest.Success())
            return;

        IsEditMode = false;

        Issue.ExecutorIdentityUserId = user_id;

        if (string.IsNullOrWhiteSpace(user_id))
            return;

        UsersIdentityDump ??= [];
        if (UsersIdentityDump.Any(x => x.UserId == user_id) != true)
        {
            await SetBusyAsync();
            TResponseModel<UserInfoModel[]> res_user = await IdentityRepo.GetUsersIdentityAsync([user_id]);
            IsBusyProgress = false;

            SnackbarRepo.ShowMessagesResponse(res_user.Messages);
            if (!res_user.Success() || res_user.Response is null || res_user.Response.Length != 1)
                return;

            UsersIdentityDump = [.. UsersIdentityDump.Union(res_user.Response)];
        }

        Executor = UsersIdentityDump?.FirstOrDefault(x => x.UserId == Issue.ExecutorIdentityUserId);
        editExecutorEmail = Executor?.Email ?? Issue.ExecutorIdentityUserId;
    }

    void EditModeToggle()
    {
        IsEditMode = !IsEditMode;
        editExecutorEmail = IsEditMode ? "" : Executor?.Email ?? Issue.ExecutorIdentityUserId;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Executor = UsersIdentityDump?.FirstOrDefault(x => x.UserId == Issue.ExecutorIdentityUserId);
        editExecutorEmail = Executor?.Email ?? Issue.ExecutorIdentityUserId;
    }
}