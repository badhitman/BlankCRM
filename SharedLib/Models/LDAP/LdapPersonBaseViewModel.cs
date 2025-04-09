////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Базовая модель персон LDAP/AD: пользователи и группы пользователей
/// </summary>
public class LdapPersonBaseViewModel(string distinguished_name, string sam_account_name, string? display_name, string common_name)
    : LdapMinimalModel(distinguished_name, sam_account_name, common_name)
{
    /// <summary>
    /// ФИО
    /// </summary>
    public string? DisplayName { get; set; } = display_name;
}