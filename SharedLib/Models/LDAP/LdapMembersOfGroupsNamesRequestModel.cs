////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// LdapMembersOfGroupsNamesRequestModel
/// </summary>
public class LdapMembersOfGroupsNamesRequestModel
{
    /// <summary>
    /// Запросы на поиск пользователей по именам групп ldap (aka коды пространств/проектов)
    /// </summary>
    public IEnumerable<string> QueriesForGroupsNames { get; set; } = [];

    /// <summary>
    /// Режим поиска/сравнения запросов
    /// </summary>
    public FindTextModesEnum Mode { get; set; }
}