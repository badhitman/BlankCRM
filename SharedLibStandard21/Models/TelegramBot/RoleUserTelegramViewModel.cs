﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RoleUserTelegramViewModel
/// </summary>
public class RoleUserTelegramViewModel
{
    /// <inheritdoc/>
    public TelegramUsersRolesEnum Role { get; set; }

    /// <inheritdoc/>
    public virtual UserTelegramViewModel? User { get; set; }

    /// <inheritdoc/>
    public int UserId { get; set; }
}