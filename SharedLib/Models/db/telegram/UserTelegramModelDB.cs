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
[PrimaryKey(nameof(Id))]
public class UserTelegramModelDB : UserTelegramViewModel
{
    /// <summary>
    /// Messages
    /// </summary>
    public List<MessageTelegramModelDB>? Messages { get; set; }

    /// <summary>
    /// ChatsJoins
    /// </summary>
    public List<JoinUserChatModelDB>? ChatsJoins { get; set; }
}