////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Roles for TelegramUser`s
/// </summary>
public enum TelegramUsersRolesEnum
{
    /// <inheritdoc/>
    [Description("Manage")]
    Manage = 100,

    /// <inheritdoc/>
    [Description("Success notifies/toast`s")]
    NotifyToastSuccess = 210,

    /// <inheritdoc/>
    [Description("Info notifies/toast`s")]
    NotifyToastInfo = 220,

    /// <inheritdoc/>
    [Description("Warning notifies/toast`s")]
    NotifyToastWarning = 230,

    /// <inheritdoc/>
    [Description("Error notifies/toast`s")]
    NotifyToastError = 240,
}