////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Net.Mail;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// ExecutorIssueComponent
/// </summary>
public partial class ExecutorIssueComponent : IssueWrapBaseModel
{
    UserInfoModel? Executor;
    string? editExecutorEmail;
    bool IsEditMode;

    async Task SetMeAsExecutor()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetExecutor(CurrentUserSession.UserId);
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
                SnackBarRepo.Error($"User with this email not found: {editExecutorEmail}");
                return;
            }

            if (user_by_email.Roles?.Any(x => GlobalStaticConstantsRoles.Roles.AllHelpDeskRoles.Contains(x)) != true && !user_by_email.IsAdmin)
            {
                SnackBarRepo.Error($"User {editExecutorEmail} cannot be installed by the performer: is not an employee");
                return;
            }
        }

        await SetExecutor(user_by_email?.UserId ?? "");
    }

    async Task SetExecutor(string user_id)
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        TResponseModel<bool> rest = await HelpDeskRepo.ExecuterUpdateAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = new() { IssueId = Issue.Id, UserId = user_id } });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(rest.Messages);

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
            TResponseModel<UserInfoModel[]> res_user = await IdentityRepo.GetUsersOfIdentityAsync([user_id]);
            IsBusyProgress = false;

            SnackBarRepo.ShowMessagesResponse(res_user.Messages);
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