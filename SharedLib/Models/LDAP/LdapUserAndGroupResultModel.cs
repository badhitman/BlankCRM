////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Результат/Модель пары пользователь+группа LDAP
/// </summary>
public class LdapUserAndGroupResultModel
{
    /// <summary>
    /// Данные пользователя
    /// </summary>
    public LdapMemberViewModel? UserData { get; set; }

    /// <summary>
    /// Данные группы
    /// </summary>
    public LdapGroupViewModel? GroupData { get; set; }
}
