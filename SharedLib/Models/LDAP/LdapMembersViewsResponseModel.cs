////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Ответ сервера с перечнем пользователей LDAP
/// </summary>
public class LdapMembersViewsResponseModel : ResponseBaseModel
{
    /// <summary>
    /// Перечень пользователей LDAP
    /// </summary>
    public IEnumerable<LdapMemberViewModel>? Members { get; set; }
}