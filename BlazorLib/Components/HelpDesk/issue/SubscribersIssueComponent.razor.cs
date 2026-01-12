////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Net.Mail;
using BlazorLib;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// Участники диалога
/// </summary>
public partial class SubscribersIssueComponent : IssueWrapBaseModel
{
    bool CanSubscribe => Issue.Subscribers?.Any(x => x.UserId == CurrentUserSession?.UserId) != true;

    string? addingSubscriber;

    async Task AddSubscriber()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (!MailAddress.TryCreate(addingSubscriber, out _))
            throw new Exception();

        await SetBusyAsync();

        TResponseModel<UserInfoModel>? user_by_email = await IdentityRepo.FindUserByEmailAsync(addingSubscriber);

        if (user_by_email.Response is null)
        {
            SnackBarRepo.Error($"Пользователь с таким email не найден: {addingSubscriber}");
            await SetBusyAsync(false);
            return;
        }

        if (!UsersIdentityDump.Any(x => x.UserId == user_by_email.Response.UserId))
            UsersIdentityDump.Add(user_by_email.Response);

        TResponseModel<bool> add_subscriber_res = await HelpDeskRepo.SubscribeUpdateAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                SetValue = true,
                IssueId = Issue.Id,
                UserId = user_by_email.Response.UserId,
            }
        });

        SnackBarRepo.ShowMessagesResponse(add_subscriber_res.Messages);
        if (!add_subscriber_res.Success() || add_subscriber_res.Response != true)
        {
            await SetBusyAsync(false);
            return;
        }

        addingSubscriber = null;

        TResponseModel<List<SubscriberIssueHelpDeskModelDB>> res = await HelpDeskRepo.SubscribesListAsync(new() { Payload = Issue.Id, SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        Issue.Subscribers = res.Response;
        await SetBusyAsync(false);
    }

    async Task NotifyBellToggle(SubscriberIssueHelpDeskModelDB p)
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        TAuthRequestStandardModel<SubscribeUpdateRequestModel> req = new()
        {
            Payload = new()
            {
                IssueId = Issue.Id,
                SetValue = true,
                UserId = p.UserId,
                IsSilent = !p.IsSilent,
            },
            SenderActionUserId = CurrentUserSession.UserId
        };

        await SetBusyAsync();

        TResponseModel<bool> rest = await HelpDeskRepo.SubscribeUpdateAsync(req);

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await SetBusyAsync(false);
            return;
        }

        TResponseModel<List<SubscriberIssueHelpDeskModelDB>> res = await HelpDeskRepo.SubscribesListAsync(new TAuthRequestStandardModel<int>() { Payload = Issue.Id, SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        Issue.Subscribers = res.Response;
        await SetBusyAsync(false);
    }

    async Task SubscribeMeToggle()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        TAuthRequestStandardModel<SubscribeUpdateRequestModel> req = new()
        {
            Payload = new()
            {
                IssueId = Issue.Id,
                SetValue = CanSubscribe,
                UserId = CurrentUserSession.UserId
            },
            SenderActionUserId = CurrentUserSession.UserId
        };

        await SetBusyAsync();

        TResponseModel<bool> rest = await HelpDeskRepo.SubscribeUpdateAsync(req);

        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            await SetBusyAsync(false);
            return;
        }
        TResponseModel<List<SubscriberIssueHelpDeskModelDB>> res = await HelpDeskRepo.SubscribesListAsync(new TAuthRequestStandardModel<int>() { Payload = Issue.Id, SenderActionUserId = CurrentUserSession.UserId });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        Issue.Subscribers = res.Response;
        await SetBusyAsync(false);
    }
}