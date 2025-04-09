////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Модель пары пользователи+группа LDAP
/// </summary>
public class LdapUsersAndGroupModel
{
    /// <summary>
    /// user ldap dn
    /// </summary>
    public IEnumerable<string> UsersDN { get; set; } = [];


    /// <summary>
    /// group ldap dn
    /// </summary>
    public string GroupDN { get; set; } = string.Empty;
}