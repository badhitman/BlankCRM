////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователь
/// </summary>
public record UserInfoModel : UserInfoMainModel
{
    /// <summary>
    /// Флаг, указывающий, подтвердил ли пользователь свой адрес электронной почты.
    /// </summary>
    /// <value>True, если адрес электронной почты был подтвержден, в противном случае — false.</value>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Получает или задает дату и время в формате UTC окончания блокировки пользователя.
    /// </summary>
    /// <remarks>
    /// Значение в прошлом означает, что пользователь не заблокирован.
    /// </remarks>
    public DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// Флаг, указывающий, может ли пользователь быть заблокирован.
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// Получает или задает количество неудачных попыток входа в систему для текущего пользователя.
    /// </summary>
    public int AccessFailedCount { get; set; }

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
    public override string ToString()
    {
        string _res = $"{GivenName} {Surname}";

        if (!UserName.EndsWith($"@{GlobalStaticConstants.FakeHost}"))
            _res = $"{_res} ({UserName}";

        if (!UserName.Equals(Email) && !Email.EndsWith($"@{GlobalStaticConstants.FakeHost}"))
            _res += $" {Email}";

        if (!string.IsNullOrWhiteSpace(PhoneNumber))
            _res += $" ({PhoneNumber})";

        return _res;
    }

    /// <inheritdoc/>
    public static UserInfoModel Build(
                string userId,
                string userName,
                string? email,
                string? phoneNumber,
                string? phoneNumberRequestChange,
                long? telegramId,
                bool emailConfirmed,
                DateTimeOffset? lockoutEnd,
                bool lockoutEnabled,
                int accessFailedCount,
                string? firstName,
                string? lastName,
                string? patronymic,
                string? externalUserId,
                string? kladrTitle,
                string? kladrCode,
                string? addressUserComment,
                string[]? roles = null,
                EntryAltModel[]? claims = null)
        => new()
        {
            GivenName = firstName,
            Surname = lastName,
            Patronymic = patronymic,
            UserId = userId,
            Email = email,
            UserName = userName,
            PhoneNumber = phoneNumber,
            ExternalUserId = externalUserId,
            RequestChangePhone = phoneNumberRequestChange,
            TelegramId = telegramId,
            EmailConfirmed = emailConfirmed,
            LockoutEnd = lockoutEnd,
            LockoutEnabled = lockoutEnabled,
            AccessFailedCount = accessFailedCount,
            Roles = roles?.ToList(),
            Claims = claims,
            KladrTitle = kladrTitle,
            KladrCode = kladrCode,
            AddressUserComment = addressUserComment,
        };

    /// <inheritdoc/>
    public static UserInfoModel BuildSystem()
    {
        return new()
        {
            UserId = GlobalStaticConstantsRoles.Roles.System,
            Email = GlobalStaticConstantsRoles.Roles.System,
            EmailConfirmed = true,
            Roles = [GlobalStaticConstantsRoles.Roles.System],
            UserName = "Система",
        };
    }

    /// <inheritdoc/>
    public static UserInfoModel BuildEmpty()
    {
        return new UserInfoModel()
        {
            UserId = "",
            UserName = "Not select",
        };
    }
}