////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// ChatsTelegramIssueComponent
/// </summary>
public partial class ChatsTelegramIssueComponent : IssueWrapBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    List<ChatTelegramViewModel>? chats = null;

    async void SendMessageTelegramAction(SendTextMessageTelegramBotModel msg)
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        PulseRequestModel req_pulse = new()
        {
            Payload = new()
            {
                Payload = new()
                {
                    Description = $"Sent a message on Telegram: user-tg#{msg.UserTelegramId}",
                    IssueId = Issue.Id,
                    PulseType = PulseIssuesTypesEnum.Messages,
                    Tag = Routes.TELEGRAM_CONTROLLER_NAME,
                },
                SenderActionUserId = CurrentUserSession.UserId
            }
        };
        await SetBusyAsync();
        await HelpDeskRepo.PulsePushAsync(req_pulse, false);
        TResponseModel<int> add_msg_system = await HelpDeskRepo.MessageCreateOrUpdateAsync(new()
        {
            SenderActionUserId = GlobalStaticConstantsRoles.Roles.System,
            Payload = new() { MessageText = $"<b>User {CurrentUserSession.UserName} sent a Telegram message to user user-tg#{msg.UserTelegramId}</b>: {msg.Message}", IssueId = Issue.Id }
        });
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(add_msg_system.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        long[] chats_ids = [.. UsersIdentityDump.Where(x => x.TelegramId.HasValue).Select(x => x.TelegramId!.Value)];

        chats = await TelegramRepo.ChatsReadTelegramAsync(chats_ids);
        await SetBusyAsync(false);
    }
}