////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Модель пары пользователь+группы LDAP
/// </summary>
public class LdapUserAndGroupsModel
{
    /// <summary>
    /// user ldap dn
    /// </summary>
    public string UserDN { get; set; } = string.Empty;


    /// <summary>
    /// group ldap dn
    /// </summary>
    public IEnumerable<string> GroupsDN { get; set; } = [];
}