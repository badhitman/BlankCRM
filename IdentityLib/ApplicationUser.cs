////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace IdentityLib;

/// <summary>
/// Пользователь [Identity]
/// </summary>
[Index(nameof(ChatTelegramId)), Index(nameof(KladrCode)), Index(nameof(RequestChangePhone)), Index(nameof(PhoneNumber)), Index(nameof(ExternalUserId))]
[Index(nameof(NormalizedFirstNameUpper)), Index(nameof(NormalizedLastNameUpper)), Index(nameof(NormalizedPatronymicUpper))]
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Telegram Id
    /// </summary>
    public long? ChatTelegramId { get; set; }

    /// <summary>
    /// FirstName
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// NormalizedFirstNameUpper
    /// </summary>
    public string? NormalizedFirstNameUpper { get; set; }

    /// <summary>
    /// LastName
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// NormalizedLastNameUpper
    /// </summary>
    public string? NormalizedLastNameUpper { get; set; }

    /// <inheritdoc/>
    public string? Patronymic { get; set; }

    /// <inheritdoc/>
    public string? NormalizedPatronymicUpper { get; set; }

    #region address
    /// <inheritdoc/>
    public string? KladrCode { get; set; }

    /// <inheritdoc/>
    public string? KladrTitle { get; set; }

    /// <summary>
    /// Адрес 
    /// </summary>
    public string? AddressUserComment { get; set; }
    #endregion

    /// <inheritdoc/>
    public string? RequestChangePhone { get; set; }

    /// <summary>
    /// Идентификатор (внешний)
    /// </summary>
    public string? ExternalUserId { get; set; }

    /// <inheritdoc/>
    public static explicit operator UserInfoModel(ApplicationUser app_user)
    {
        return UserInfoModel.Build(
            userId: app_user.Id,
            userName: app_user.UserName ?? "",
            email: app_user.Email,
            phoneNumber: app_user.PhoneNumber,
            phoneNumberRequestChange: app_user.RequestChangePhone,
            telegramId: app_user.ChatTelegramId,
            emailConfirmed: app_user.EmailConfirmed,
            lockoutEnd: app_user.LockoutEnd,
            lockoutEnabled: app_user.LockoutEnabled,
            accessFailedCount: app_user.AccessFailedCount,
            firstName: app_user.FirstName,
            lastName: app_user.LastName,
            patronymic: app_user.Patronymic,
            kladrTitle: app_user.KladrTitle,
            kladrCode: app_user.KladrCode,
            externalUserId: app_user.ExternalUserId,
            addressUserComment: app_user.AddressUserComment);
    }
}