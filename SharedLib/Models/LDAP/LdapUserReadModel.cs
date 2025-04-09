////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователь LDAP
/// </summary>
public class LdapUserReadModel
{
    /// <summary>
    /// DN пользоваля LDAP:AD
    /// </summary>
    public string UserDN { get; set; } = string.Empty;

    /// <summary>
    /// Имя пользователя LDAP:AD
    /// </summary>
    public string Username { get; set; } = string.Empty;
}