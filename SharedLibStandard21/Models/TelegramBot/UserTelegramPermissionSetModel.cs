////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// UserTelegramPermissionSetModel
/// </summary>
public class UserTelegramPermissionSetModel
{
    /// <inheritdoc/>
    public TelegramUsersRolesEnum[]? Roles { get; set; }

    /// <inheritdoc/>
    public int UserId { get; set; }
}