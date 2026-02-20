////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Модель пары пользователь+группа LDAP
/// </summary>
public class LdapUserAndGroupModel
{
    /// <summary>
    /// user ldap dn
    /// </summary>
    public string UserDN { get; set; } = string.Empty;


    /// <summary>
    /// group ldap dn
    /// </summary>
    public string GroupDN { get; set; } = string.Empty;
}