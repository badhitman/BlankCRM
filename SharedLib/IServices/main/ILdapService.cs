using SharedLib;

namespace SharedLib;

/// <summary>
/// ILdapService
/// </summary>
public interface ILdapService
{
    /// <summary>
    /// Логин LDAP
    /// </summary>
    public string Login { get; protected set; }
    /// <summary>
    /// User LDAP
    /// </summary>
    public string User { get; set; }
    /// <summary>
    /// Пароль LDAP
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Получить группы пользователя
    /// </summary>
    /// <param name="base_filters">Перечень базовых фильтров для поиска</param>
    /// <param name="token"></param>
    /// <param name="user_dn">Имя пользователя (полный адрес/путь)</param>
    public Task<IEnumerable<string>> GetGroupsDNForUser(string user_dn, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Создать пользователя
    /// </summary>
    public Task<ResponseBaseModel> CreateUser(LdapUserInformationModel user, CancellationToken token = default);

    /// <summary>
    /// Перевыпуск пароля пользователю и отправка уведомления (с новым паролем) ему
    /// </summary>
    /// <param name="sAMAccountName">Логин пользователя</param>
    /// <param name="token"></param>
    public Task<ResponseBaseModel> RenewPassword(string sAMAccountName, CancellationToken token = default);

    /// <summary>
    /// Получить метаданные (sAMAccountName и distinguishedName) групп пользователя
    /// </summary>
    /// <param name="user_name">sAMAccountName пользователя</param>
    /// <param name="base_filters">Базовые фильтры для групп</param>
    /// <param name="token"></param>
    /// <returns>Метаданные (sAMAccountName и distinguishedName) групп пользователя</returns>
    public Task<IEnumerable<LdapPersonBaseViewModel>> GetMetadataGroupsForUser(string user_name, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Данные пользователей по перечню sAMAccountName
    /// </summary>
    /// <param name="samaccounts_names">Перечень sAMAccountName пользователей</param>
    /// <param name="base_filters">Базовый фильтр (ограничение поиска)</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapMemberViewModel>> GetMembersViewDataBySamAccountNames(IEnumerable<string> samaccounts_names, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить участников группы (лёгкая модель ответа)
    /// </summary>
    /// <param name="group_dn">Имя группы (полный адрес/путь)</param>
    /// <param name="token"></param>
    /// <returns>Результат выполнения запроса</returns>
    public Task<IEnumerable<LdapMemberViewModel>> GetMembersViewOfGroup(string group_dn, CancellationToken token = default);

    /// <summary>
    /// Найти пользователей по E-mail адресам
    /// </summary>
    /// <param name="emails">Искомые E-mail адреса</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    /// <returns>Найденые пользователи</returns>
    public Task<IEnumerable<LdapMemberViewModel>> FindMembersOfEmails(IEnumerable<string> emails, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить всех пользователей (кроме сервисных) с установленным Email
    /// </summary>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    /// <returns>Найденые пользователи</returns>
    public Task<IEnumerable<LdapMemberViewModel>> GetAllEmails(IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить пользователей групп
    /// </summary>
    /// <param name="groups_dn_memberOf">Группы (DN)</param>
    /// <param name="token"></param>
    /// <returns>Пользователи</returns>
    public Task<IEnumerable<LdapMemberViewModel>> GetMembersOfGroups(IEnumerable<string> groups_dn_memberOf, CancellationToken token = default);

    /// <summary>
    /// Получить перечень OU (organizationalUnit)
    /// </summary>
    /// <param name="req">запрос поиска LDAP</param>
    /// <param name="token"></param>
    /// <returns>перечень OU (organizationalUnit)</returns>
    public Task<IEnumerable<string>> FindOrgUnits(LdapSimpleRequestModel req, CancellationToken token = default);

    /// <summary>
    /// поиск групп по имени CN
    /// </summary>
    /// <param name="queries_cn">Запросы к CN группы</param>
    /// <param name="base_filters">базовое ограничение/фильтр пути/dn</param>
    /// <param name="is_scope_sub">Вложенный поиск: область поиска записей включает поиск базового объекта и всех записей в его поддереве</param>
    /// <param name="token"></param>
    /// <returns>Группы по запросу</returns>
    public Task<IEnumerable<LdapMinimalModel>> FindGroupsDNByCN(IEnumerable<string> queries_cn, IEnumerable<string> base_filters, bool is_scope_sub = true, CancellationToken token = default);

    /// <summary>
    /// Найти DN групп по поиску строки в CN
    /// </summary>
    /// <param name="queries_cn">Текст поиска в CN искомой группы</param>
    /// <param name="mode">Режим поиска</param>
    /// <param name="base_filters">базовое ограничение/фильтр пути/dn</param>
    /// <param name="token"></param>
    /// <returns>Резульатт запроса</returns>
    public Task<IEnumerable<string>> FindGroupsDNByQueryCN(IEnumerable<string> queries_cn, FindTextModesEnum mode, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Поиск DN групп по коду пространства (поиск по полю: cn)
    /// </summary>
    /// <param name="code_template">Искомая строка для поиска</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    /// <returns>Результат выполнения запроса с подходящими группами</returns>
    public Task<IEnumerable<string>> FindGroupsDNByCodeTemplate(string code_template, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Поиск пользователей групп по произвольной строке в CN группы
    /// </summary>
    /// <param name="queries_for_groups_names">Искомые строки для поиска групп по имени (пользователи которых требуются)</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    /// <param name="mode">режим сравнения строки LDAP</param>
    /// <returns>Результат выполнения запроса с подходящими группами</returns>
    public Task<IEnumerable<LdapMemberViewModel>> FindMembersOfGroupsByQueryGroupName(IEnumerable<string> queries_for_groups_names, FindTextModesEnum mode, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Подбор участников групп по строке поиска: поиск в CN (фио), SAMAccountName, email
    /// </summary>
    /// <param name="query">Строка запроса поиска пользователя LDAP (поиск в CN и/или email)</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    /// <returns>Результат выполнения запроса с подходящими пользователями</returns>
    public Task<IEnumerable<LdapMemberViewModel>> FindMembersViewDataByQuery(IEnumerable<string> query, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить информацию о группах по перечню sAMAccountName
    /// </summary>
    /// <param name="groups_sam_account_names">Перечень sAMAccountName групп</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapGroupViewModel>> GetGroupsBySAMAccountNames(IEnumerable<string> groups_sam_account_names, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить информацию о группах по перечню DistinguishedName
    /// </summary>
    /// <param name="groups_dns">Перечень DistinguishedName групп</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapGroupViewModel>> GetGroupsByDistinguishedNames(IEnumerable<string> groups_dns, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить информацию о группах по перечню DistinguishedName групп-владельцев
    /// </summary>
    /// <param name="groups_dns">Перечень DistinguishedName групп</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapGroupViewModel>> GetGroupsByParentDistinguishedNames(IEnumerable<string> groups_dns, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить информацию о пользователях по перечню sAMAccountName
    /// </summary>
    /// <param name="users_sam_account_names">Перечень sAMAccountName пользователей</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapMemberViewModel>> GetMembersBySAMAccountNames(IEnumerable<string> users_sam_account_names, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить информацию о пользователях по перечню DistinguishedName
    /// </summary>
    /// <param name="users_dns">Перечень DistinguishedName пользователей</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapMemberViewModel>> GetUsersByDistinguishedNames(IEnumerable<string> users_dns, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Поиск групп по строке запроса: в CN, name и sAMAccountName
    /// </summary>
    /// <param name="query">Строка запроса поиска (поиск в: CN, name и sAMAccountName)</param>
    /// <param name="base_filters">фильтры поиска LDAP</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapGroupViewModel>> FindGroups(string query, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Добавить пользователя в группу
    /// </summary>
    /// <param name="user_dn">пользователь</param>
    /// <param name="group_dn">группа</param>
    /// <param name="base_filters_users">Ограничение фильтра поиска пользователя</param>
    /// <param name="base_filters_groups">Ограничение фильтра поиска группы</param>
    /// <param name="token"></param>
    /// <returns>Итоговый список участников группы послое попытки добавить туда нового участника</returns>
    public Task<LdapMembersViewsResponseModel> InjectMemberToGroup(string user_dn, string group_dn, IEnumerable<string>? base_filters_users = null, IEnumerable<string>? base_filters_groups = null, CancellationToken token = default);

    /// <summary>
    /// Удалить пользователя из группы
    /// </summary>
    /// <param name="user_dn">пользователь</param>
    /// <param name="group_dn">группа</param>
    /// <param name="base_filters_users">Ограничение фильтра поиска пользователя</param>
    /// <param name="base_filters_groups">Ограничение фильтра поиска группы</param>
    /// <param name="token"></param>
    /// <returns>Итоговый список участников группы послое попытки удалить оттуда участника</returns>
    public Task<LdapMembersViewsResponseModel> KickMemberFromGroup(string user_dn, string group_dn, IEnumerable<string>? base_filters_users = null, IEnumerable<string>? base_filters_groups = null, CancellationToken token = default);

    /// <summary>
    /// Проверить пользователя и/или группу на наличие в LDAP. Проверяться будет только заполненное DN поле.
    /// </summary>
    /// <param name="inc">исходные/запрашиваемые/проверяемые DN объектов</param>
    /// <param name="base_filters_users">базовые фильтры для поиска пользователя</param>
    /// <param name="base_filters_groups">базовые фильтры для поиска группы</param>
    /// <param name="token"></param>
    /// <returns>Возвращает либо те же запрашиваемые DN (если они обнаружены в LDAP), либо пустую строку (если в LDAP такого не найдено)</returns>
    public Task<LdapUserAndGroupResultModel> CheckUserGroupPair(LdapUserAndGroupModel inc, IEnumerable<string>? base_filters_users = null, IEnumerable<string>? base_filters_groups = null, CancellationToken token = default);

    /// <summary>
    /// Отключить/Включить пользователя (+ переместить в соответствующую OU)
    /// </summary>
    /// <param name="sAMAccountName">Логин AD</param>
    /// <param name="setUserIsDisabled">Статус который следует установить пользователю (вкл/выкл). true - пользователь отключён, а false - пользователь не отключён</param>
    /// <param name="newOuDistinguishedName">Новая OU для перемещения (если не указано, то перемещения не будет)</param>
    /// <param name="base_filters_users">базовые фильтры для поиска пользователя</param>
    /// <param name="token"></param>
    /// <returns>Результат выполнения запроса</returns>
    public Task<ResponseBaseModel> SetDisableStateUser(string sAMAccountName, bool setUserIsDisabled, string? newOuDistinguishedName, IEnumerable<string>? base_filters_users = null, CancellationToken token = default);

    /// <summary>
    /// Установить Telegram ID для пользователя
    /// </summary>
    /// <param name="user_dn">Пользователь (distinguishedName)</param>
    /// <param name="telegram_id">Telegram ID</param>
    /// <param name="token"></param>
    public Task<ResponseBaseModel> SetTelegramIdForUser(string user_dn, string telegram_id, CancellationToken token = default);

    /// <summary>
    /// Получить данные пользователей по перечню Email`s
    /// </summary>
    /// <param name="emails">Перечень Email-ов для поиска пользователей</param>
    /// <param name="base_filters">Базовый фильтр (ограничение поиска)</param>
    /// <param name="token"></param>
    public Task<IEnumerable<LdapMemberViewModel>> GetMembersViewDataByEmails(IEnumerable<string> emails, IEnumerable<string>? base_filters = null, CancellationToken token = default);

    /// <summary>
    /// Получить данные пользователей по перечню Telegram Id`s
    /// </summary>
    /// <param name="ids">Перечень Telegram Id`s для поиска пользователей</param>
    /// <param name="base_filters">Базовый фильтр (ограничение поиска)</param>
    /// <param name="token"></param>
    public Task<List<LdapMemberViewModel>> GetMembersViewDataByTelegramIds(IEnumerable<string> ids, IEnumerable<string>? base_filters = null, CancellationToken token = default);
}