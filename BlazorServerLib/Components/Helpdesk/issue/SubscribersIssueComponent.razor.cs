﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Net.Mail;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Helpdesk.issue;

/// <summary>
/// Участники диалога
/// </summary>
public partial class SubscribersIssueComponent : IssueWrapBaseModel
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    bool CanSubscribe => Issue.Subscribers?.Any(x => x.UserId == CurrentUserSession!.UserId) != true;

    string? addingSubscriber;

    async Task AddSubscriber()
    {
        if (!MailAddress.TryCreate(addingSubscriber, out _))
            throw new Exception();

        await SetBusyAsync();

        TResponseModel<UserInfoModel>? user_by_email = await IdentityRepo.FindUserByEmailAsync(addingSubscriber);
        IsBusyProgress = false;
        if (user_by_email.Response is null)
        {
            SnackbarRepo.Error($"Пользователь с таким email не найден: {addingSubscriber}");
            return;
        }

        if (!UsersIdentityDump.Any(x => x.UserId == user_by_email.Response.UserId))
            UsersIdentityDump.Add(user_by_email.Response);

        await SetBusyAsync();

        TResponseModel<bool> add_subscriber_res = await HelpdeskRepo.SubscribeUpdateAsync(new()
        {
            SenderActionUserId = CurrentUserSession!.UserId,
            Payload = new()
            {
                SetValue = true,
                IssueId = Issue.Id,
                UserId = user_by_email.Response.UserId,
            }
        });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(add_subscriber_res.Messages);
        if (!add_subscriber_res.Success() || add_subscriber_res.Response != true)
            return;

        addingSubscriber = null;
        await SetBusyAsync();
        TResponseModel<List<SubscriberIssueHelpdeskModelDB>> res = await HelpdeskRepo.SubscribesListAsync(new() { Payload = Issue.Id, SenderActionUserId = CurrentUserSession!.UserId });
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        Issue.Subscribers = res.Response;
        IsBusyProgress = false;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    async Task NotifyBellToggle(SubscriberIssueHelpdeskModelDB p)
    {
        TAuthRequestModel<SubscribeUpdateRequestModel> req = new()
        {
            Payload = new()
            {
                IssueId = Issue.Id,
                SetValue = true,
                UserId = p.UserId,
                IsSilent = !p.IsSilent,
            },
            SenderActionUserId = CurrentUserSession!.UserId
        };

        await SetBusyAsync();

        TResponseModel<bool> rest = await HelpdeskRepo.SubscribeUpdateAsync(req);

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
        {
            IsBusyProgress = false;
            return;
        }

        TResponseModel<List<SubscriberIssueHelpdeskModelDB>> res = await HelpdeskRepo.SubscribesListAsync(new TAuthRequestModel<int>() { Payload = Issue.Id, SenderActionUserId = CurrentUserSession!.UserId });
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        Issue.Subscribers = res.Response;
        IsBusyProgress = false;
    }

    async Task SubscribeMeToggle()
    {
        TAuthRequestModel<SubscribeUpdateRequestModel> req = new()
        {
            Payload = new()
            {
                IssueId = Issue.Id,
                SetValue = CanSubscribe,
                UserId = CurrentUserSession!.UserId
            },
            SenderActionUserId = CurrentUserSession!.UserId
        };

        await SetBusyAsync();

        TResponseModel<bool> rest = await HelpdeskRepo.SubscribeUpdateAsync(req);

        SnackbarRepo.ShowMessagesResponse(rest.Messages);
        if (!rest.Success())
            return;
        TResponseModel<List<SubscriberIssueHelpdeskModelDB>> res = await HelpdeskRepo.SubscribesListAsync(new TAuthRequestModel<int>() { Payload = Issue.Id, SenderActionUserId = CurrentUserSession!.UserId });
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        Issue.Subscribers = res.Response;
        IsBusyProgress = false;
    }
}