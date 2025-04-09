////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Модель группы LDAP для отображения
/// </summary>
public class LdapGroupClientModel
{
    /// <summary>
    /// DN
    /// </summary>
    public string DistinguishedName { get; set; } = string.Empty;

    /// <summary>
    /// Количество участников в группе
    /// </summary>
    public int? MembersCount { get; set; } = null;

    /// <summary>
    /// Выбран (активный) в списке
    /// </summary>
    public bool IsActive { get; set; }
}