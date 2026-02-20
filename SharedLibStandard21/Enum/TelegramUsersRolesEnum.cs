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
    [Description("Success toast`s")]
    NotifyToastSuccess = 210,

    /// <inheritdoc/>
    [Description("Info toast`s")]
    NotifyToastInfo = 220,

    /// <inheritdoc/>
    [Description("Warning toast`s")]
    NotifyToastWarning = 230,

    /// <inheritdoc/>
    [Description("Error toast`s")]
    NotifyToastError = 240,
}