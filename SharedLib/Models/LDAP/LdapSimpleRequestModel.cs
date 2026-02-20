////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Простой запрос к LDAP
/// </summary>
public class LdapSimpleRequestModel
{
    /// <summary>
    /// Запрос
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Ограничивающие OU базовые фильтры (distinguishedName`s)
    /// </summary>
    public IEnumerable<string>? BaseFilters { get; set; } = Enumerable.Empty<string>();
}