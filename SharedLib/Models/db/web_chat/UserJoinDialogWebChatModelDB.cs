////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// UserJoinDialogModelDB
/// </summary>
[Index(nameof(UserIdentityId)), Index(nameof(JoinedDateUTC)), Index(nameof(OutDateUTC))]
public class UserJoinDialogWebChatModelDB : UserInjectDialogWebChatBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Когда пользователь подключился к чату
    /// </summary>
    public required DateTime JoinedDateUTC { get; set; }

    /// <summary>
    /// Когда пользователь покинул чат
    /// </summary>
    public DateTime? OutDateUTC { get; set; }

    /// <inheritdoc/>
    public DialogWebChatModelDB? DialogJoin { get; set; }

    /// <inheritdoc/>
    public static UserJoinDialogWebChatModelDB Build(UserInjectDialogWebChatRequestModel payload)
    {
        return new()
        {
            JoinedDateUTC = DateTime.UtcNow,
            UserIdentityId = payload.UserIdentityId,
            DialogJoinId = payload.DialogJoinId,
        };
    }
}