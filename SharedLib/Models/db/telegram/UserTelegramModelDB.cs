////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// UserTelegramModelDB
/// </summary>
[Index(nameof(UserTelegramId), IsUnique = true)]
[Index(nameof(Username)), Index(nameof(FirstName)), Index(nameof(LastName)), Index(nameof(IsBot))]
public class UserTelegramModelDB : UserTelegramStandardModel
{
    /// <summary>
    /// Messages
    /// </summary>
    public List<MessageTelegramModelDB>? Messages { get; set; }

    /// <summary>
    /// ChatsJoins
    /// </summary>
    public new List<JoinUserChatModelDB>? ChatsJoins { get; set; }
}