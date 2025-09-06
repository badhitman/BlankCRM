////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// JoinUserChatModelDB
/// </summary>
[Index(nameof(UserId), nameof(ChatId), IsUnique = true)]
public class JoinUserChatModelDB : JoinUserChatViewModel
{
    /// <summary>
    /// User
    /// </summary>
    public new UserTelegramModelDB? User { get; set; }

    /// <summary>
    /// Chat
    /// </summary>
    public new ChatTelegramModelDB? Chat { get; set; }
}