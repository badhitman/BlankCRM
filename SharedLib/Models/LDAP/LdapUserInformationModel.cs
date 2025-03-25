////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Информация о LDAP/AD пользователе
/// </summary>
public class LdapUserInformationModel : UserInformationModel
{
    /// <summary>
    /// Organizational unit (organizationalUnit)
    /// </summary>
    public required string OU { get; set; }

    /// <summary>
    /// Telegram id (опционально)
    /// </summary>
    public string? TelegramId { get; set; }
}