////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// NotificationsAreaTelegramIssueConfigComponent
/// </summary>
public partial class NotificationsAreaTelegramIssueConfigComponent : IssueWrapBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    List<ChatTelegramViewModel>? chatsTelegram;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        long[] telegram_users_ids = [.. UsersIdentityDump
            .Where(x => x.TelegramId.HasValue)
            .Select(x => x.TelegramId!.Value)];

        if (telegram_users_ids.Length == 0)
            return;

        await SetBusyAsync();
        chatsTelegram = [.. await TelegramRepo.ChatsFindForUserTelegramAsync(telegram_users_ids)];

        chatsTelegram.Insert(0, new() { Title = "Off", Type = ChatsTypesTelegramEnum.Private });

        await SetBusyAsync(false);
    }
}