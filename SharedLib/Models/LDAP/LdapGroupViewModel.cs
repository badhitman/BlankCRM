////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Группы LDAP/AD
/// </summary>
public class LdapGroupViewModel(string distinguished_name, string sam_account_name, string common_name, string name)
    : LdapMinimalModel(distinguished_name, sam_account_name, common_name)
{
    /// <summary>
    /// name атрибут ad:ldap 
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Участники/пользователи группы (distinguishedName`s)
    /// </summary>
    public IEnumerable<string>? MembersDn { get; set; }
}