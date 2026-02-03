////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// UserInjectDialogWebChatBaseModel
/// </summary>
public class UserInjectDialogWebChatBaseModel
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public required string UserIdentityId { get; set; }

    /// <inheritdoc/>
    public int DialogJoinId { get; set; }
}
