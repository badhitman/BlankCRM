////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// UserJoinDialogModelDB
/// </summary>
[Index(nameof(UserIdentityId), nameof(DialogJoinId), IsUnique = true)]
public class UserJoinDialogWebChatModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public required string UserIdentityId { get; set; }

    /// <inheritdoc/>
    public DialogWebChatModelDB? DialogJoin { get; set; }
    /// <inheritdoc/>
    public int DialogJoinId { get; set; }
}