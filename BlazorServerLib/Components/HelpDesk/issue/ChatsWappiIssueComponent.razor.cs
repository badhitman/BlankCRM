﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorWebLib.Components.HelpDesk.issue;

/// <summary>
/// ChatsWappiIssueComponent
/// </summary>
public partial class ChatsWappiIssueComponent : IssueWrapBaseModel
{
    string[]? chats = null;

    async void SendMessageWhatsAppAction(EntryAltExtModel msg)
    {
        PulseRequestModel req_pulse = new()
        {
            Payload = new()
            {
                Payload = new()
                {
                    Description = $"Sent a message to WhatsApp: {msg.Number}<hr/>{msg.Text}",
                    IssueId = Issue.Id,
                    PulseType = PulseIssuesTypesEnum.Messages,
                    Tag = Routes.WAPPI_CONTROLLER_NAME,
                },
                SenderActionUserId = CurrentUserSession!.UserId
            }
        };
        await SetBusyAsync();
        TResponseModel<int> add_msg_system = default!;
        List<Task> tasks = [HelpDeskRepo.PulsePushAsync(req_pulse, false),
        Task.Run(async () => {
            add_msg_system = await HelpDeskRepo.MessageCreateOrUpdateAsync(new() { SenderActionUserId = GlobalStaticConstantsRoles.Roles.System, Payload = new() { MessageText = $"<b>Пользователь {CurrentUserSession!.UserName} отправил сообщение WhatsApp пользователю {msg.Number}</b>: {msg.Text}", IssueId = Issue.Id }});
        })];
        await Task.WhenAll(tasks);
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(add_msg_system.Messages);
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        chats = [.. UsersIdentityDump.Where(x => GlobalTools.IsPhoneNumber(x.PhoneNumber)).Select(x => x.PhoneNumber)!];
    }
}