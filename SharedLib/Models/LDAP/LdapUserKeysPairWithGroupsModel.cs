////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользовательская пара идентификаторов (distinguishedName и sAMAccountName) + перечень груп (DNs)
/// </summary>
/// <remarks>
/// Пользовательская пара идентификаторов (distinguishedName и sAMAccountName) + перечень груп (DNs)
/// </remarks>
public class LdapUserKeysPairWithGroupsModel(string distinguished_name, string sam_account_name, string display_name, string common_name, IEnumerable<string> groups_dn)
    : LdapPersonBaseViewModel(distinguished_name: distinguished_name, sam_account_name: sam_account_name, display_name: display_name, common_name: common_name)
{
    /// <summary>
    /// Группы
    /// </summary>
    public IEnumerable<string> GroupsDNs { get; set; } = groups_dn;
}