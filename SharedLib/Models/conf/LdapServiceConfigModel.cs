namespace SharedLib;

/// <summary>
/// Конфигурация служб бэка обслуживания LDAP
/// </summary>
public class LdapServiceConfigModel
{
    /// <summary>
    /// OUTemporary
    /// </summary>
    public string? OUTemporary {  get; set; }

    /// <summary>
    /// Набор фильтров LDAP для поиска групп
    /// </summary>
    public string[] LdapBasedFiltersForFindGroups { get; set; } = [];

    /// <summary>
    /// Набор фильтров LDAP для поиска пользователей
    /// </summary>
    public string[] LdapBasedFiltersForFindUsers { get; set; } = [];

    /// <summary>
    /// Набор фильтров LDAP для получения групп пользователей -> RBAC.
    /// Эти группы транслируются в роли Identity (KK-&lt;GROUP&gt;)
    /// </summary>
    public string[] LdapBasedFiltersForGroupsAsRoles { get; set; } = [];

    /// <summary>
    /// Лимит на количество строк в ответе на запросы
    /// </summary>
    public int LimitResponseRows { get; set; } = 100;

    /// <summary>
    /// Базовые ограничения поиска PermissionGroups / organizationalUnit
    /// </summary>
    public string[] PermissionGroups { get; set; } = [];

    /// <summary>
    /// Указано, будут ли referrals отслеживаться автоматически.
    ///    True, если referrals следуют автоматически, или false, если referrals вызывают LdapReferralException
    /// </summary>
    public bool ReferralFollowing { get; set; }
}