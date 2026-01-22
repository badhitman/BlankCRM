////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// ChatTelegramModelDB
/// </summary>
[Index(nameof(ChatTelegramId), IsUnique = true), Index(nameof(NormalizedFirstNameUpper)), Index(nameof(LastUpdateUtc))]
[Index(nameof(NormalizedLastNameUpper)), Index(nameof(NormalizedTitleUpper)), Index(nameof(NormalizedUsernameUpper))]
[Index(nameof(Type)), Index(nameof(Title)), Index(nameof(Username)), Index(nameof(FirstName)), Index(nameof(LastName)), Index(nameof(IsForum))]
public class ChatTelegramModelDB : ChatTelegramStandardModel
{
    /// <summary>
    /// Optional. Title, for supergroups, channels and group chats
    /// </summary>
    public string? NormalizedTitleUpper { get; set; }

    /// <summary>
    /// Optional. Username, for private chats, supergroups and channels if available
    /// </summary>
    public string? NormalizedUsernameUpper { get; set; }

    /// <summary>
    /// Optional. First name of the other party in a private chat
    /// </summary>
    public string? NormalizedFirstNameUpper { get; set; }

    /// <summary>
    /// Optional. Last name of the other party in a private chat
    /// </summary>
    public string? NormalizedLastNameUpper { get; set; }


    /// <summary>
    /// LastMessageId
    /// </summary>
    public int LastMessageId { get; set; }

    /// <summary>
    /// ChatPhoto
    /// </summary>
    public ChatPhotoTelegramModelDB? ChatPhoto { get; set; }

    /// <summary>
    /// Messages
    /// </summary>
    public new List<MessageTelegramModelDB>? Messages { get; set; }

    /// <summary>
    /// ChatsJoins
    /// </summary>
    public new List<JoinUserChatModelDB>? UsersJoins { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string res = Id < 1 ? "" : $"[{Type.DescriptionInfo()}]";

        if (!string.IsNullOrWhiteSpace(Title))
            res += $" /{Title.Trim()}/";

        if (!string.IsNullOrWhiteSpace(Username))
            res += $" (@{Username.Trim()})";

        return $"{$"{FirstName} {LastName}".Trim()} {res}".Trim();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is ChatTelegramModelDB ct)
            return
                ct.Title == Title &&
                ct.LastName == LastName &&
                ct.ChatTelegramId == ChatTelegramId &&
                ct.FirstName == FirstName &&
                ct.Id == Id &&
                ct.Type == Type &&
                ct.Username == Username;

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return $"{Id}{ChatTelegramId}{Type}{Title}{Username}{FirstName}{LastName}{IsForum}".GetHashCode();
    }
}