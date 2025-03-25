////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Security.Cryptography.X509Certificates;
using Novell.Directory.Ldap;
using System.Net.Security;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;
using SharedLib;

namespace LdapService;

/// <inheritdoc/>
public record struct LdapOptionsRecModel(string Ip, string Host, string BasePath, bool UseSSL);

public class LdapService : ILdapService, IDisposable
{
    /// <inheritdoc/>
    protected LdapConnection? ldap_conn = null;

    /// <summary>
    /// Logger
    /// </summary>
    public required virtual ILogger<LdapService> _logger { get; set; }
    /// <summary>
    /// Config ldap service
    /// </summary>
    public required LdapServiceConfigModel _config_ldap_service { get; set; }

    /// <inheritdoc/>
    public required LdapOptionsRecModel _ad_opt;

    /// <inheritdoc/>
    public string User { get; set; } = string.Empty;
    /// <inheritdoc/>
    public string Login { get; set; } = string.Empty;
    /// <inheritdoc/>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// без этой корректировки поиск пользователей выполняется очень медленно
    /// </summary>
    const string findMembersPrefix = "OU=Users,";

    /// <inheritdoc/>
    protected string BasePath => !string.IsNullOrWhiteSpace(_ad_opt.BasePath) ? _ad_opt.BasePath : string.Join(',', (_ad_opt.Host ?? string.Empty).Split('.').Select(x => $"DC={x}"));

    /// <summary>
    /// Не пользователь, а служебная у/з
    /// </summary>
    static uint UacDontHumanFlags => ACCOUNTDISABLE | LOCKOUT | PASSWD_NOTREQD | PASSWD_CANT_CHANGE | DONT_EXPIRE_PASSWORD | PASSWORD_EXPIRED;

    #region LDAP Constants

    /// <inheritdoc/>
    protected const string SAMAccountNameAttribute = "sAMAccountName", DistinguishedNameAttribute = "distinguishedName", CnNameAttribute = "cn", MailNameAttribute = "mail",
        TelegramIdAttribute = "telegramID", NameAttribute = "name", DescriptionNameAttribute = "description", DisplayNameAttribute = "displayName", MemberOfNameAttribute = "memberOf",
        LastLogonTimestampNameAttribute = "lastLogonTimestamp", WhenChangedNameAttribute = "whenChanged", WhenCreatedNameAttribute = "whenCreated", UnicodePwdNameAttribute = "unicodePwd",
        PwdLastSetNameAttribute = "pwdLastSet", UserAccountControlAttribute = "userAccountControl", MemberNameAttribute = "member", LDAP_MATCHING_RULE_BIT_OR = "1.2.840.113556.1.4.804",
        GivenNameNameAttribute = "givenName", SnNameAttribute = "sn", msDS_UserPasswordExpiryTimeComputedNameAttribute = "msDS-UserPasswordExpiryTimeComputed",
        msDS_UserAccountControlComputedNameAttribute = "msDS-User-Account-Control-Computed", LastLogonNameAttribute = "lastLogon", LockoutTimeNameAttribute = "lockoutTime";

    const uint ACCOUNTDISABLE = 0x0002, LOCKOUT = 0x0010, PASSWD_NOTREQD = 0x0020, PASSWD_CANT_CHANGE = 0x0040, DONT_EXPIRE_PASSWORD = 0x10000, PASSWORD_EXPIRED = 0x800000;

    #endregion LDAP Constants

    /// <inheritdoc/>
    public async Task Connect(CancellationToken token = default)
    {
        if (ldap_conn?.Connected == true)
            ldap_conn.Disconnect();

        if (string.IsNullOrWhiteSpace(Password))
        {
            _logger.LogError($"Не установлен пароль ldap/ad. Подключение невозмоно");
            return;
        }

        string end_point_connect;
        if (_ad_opt.UseSSL)
        {
            LdapConnectionOptions o = new();
            o.ConfigureRemoteCertificateValidationCallback(
                (object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    if (sslPolicyErrors != SslPolicyErrors.None && sslPolicyErrors != SslPolicyErrors.RemoteCertificateNameMismatch)
                        _logger.LogDebug($"{_ad_opt.Host}: {sslPolicyErrors}, {certificate?.Subject}, {certificate?.Issuer}");

                    return true;
                });
            o.UseSsl();

            ldap_conn = new LdapConnection(o);
            end_point_connect = (string.IsNullOrWhiteSpace(_ad_opt.Ip) ? _ad_opt.Host : _ad_opt.Ip);
            _logger.LogDebug($"try ldap connect (SSL): {end_point_connect}");

            await ldap_conn.ConnectAsync(end_point_connect, LdapConnection.DefaultSslPort, token);
        }
        else
        {
            ldap_conn = new LdapConnection();
            end_point_connect = (!string.IsNullOrWhiteSpace(_ad_opt.Ip) ? _ad_opt.Ip : _ad_opt.Host);
            _logger.LogDebug($"try ldap connect (no ssl): {end_point_connect}");
            await ldap_conn.ConnectAsync(end_point_connect, LdapConnection.DefaultPort);
        }

        if (!ldap_conn.Connected)
        {
            _logger.LogError($"Не удалось подключиться ldap/ad");
            return;
        }

        string user_dn = !string.IsNullOrWhiteSpace(Login) ? Login : (User + "@" + _ad_opt.Host);
        _logger.LogDebug($"try binding ldap: {JsonConvert.SerializeObject(new { User, Login, end_point_connect, user_dn, ph = GlobalTools.GetHashString(Password) })}");
        await ldap_conn.BindAsync(user_dn, Password);

        LdapSearchConstraints cons = ldap_conn.SearchConstraints;
        cons.MaxResults = _config_ldap_service.LimitResponseRows;
        cons.ReferralFollowing = _config_ldap_service.ReferralFollowing;
        ldap_conn.Constraints = cons;
        return;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> CreateUserAsync(LdapUserInformationModel user, CancellationToken token = default)
    {
        string msg = "Не удачная попытка создать пользователя в AD: ";
        if (!MailAddress.TryCreate(user.Email, out _) || string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.Login))
        {
            msg = $"{msg}не корректные данные (email, firs_name и login обязательные поля) - `{JsonConvert.SerializeObject(user)}`";
            _logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        if (string.IsNullOrWhiteSpace(user.OU) && !string.IsNullOrWhiteSpace(_config_ldap_service.OUTemporary))
        {
            _logger.LogInformation($"В запросе на создание пользователя `{user.Login}` не указан `OU`. Установлена `OU` для временного размещения: {_config_ldap_service.OUTemporary}");
            user.OU = _config_ldap_service.OUTemporary;
        }
        //string subject_mail = "Создана учётная запись", message_template = Properties.Resources.create_user_mail;
        //MailTemplateModelDB? mail_tmpl;
        //if (user.UseEmailThemplateId > 0)
        //{
        //    mail_tmpl = await _prop_context.MailTemplates.FirstOrDefaultAsync(x => x.Id == user.UseEmailThemplateId);
        //    if (mail_tmpl is null)
        //    {
        //        msg = $"{msg}ошибка шаблона письма #{user.UseEmailThemplateId} (с таким ID не найден)";
        //        _logger.LogError(msg);
        //        return ResponseBaseModel.CreateError(msg);
        //    }
        //    if (!mail_tmpl.Body.Contains("[accountname]", StringComparison.OrdinalIgnoreCase) || !mail_tmpl.Body.Contains("[password]", StringComparison.OrdinalIgnoreCase))
        //    {
        //        msg = $"{msg}ошибка шаблона письма #{user.UseEmailThemplateId} (в теле шаблона не найдены: [accountname] и/или [password])";
        //        _logger.LogError(msg);
        //        return ResponseBaseModel.CreateError(msg);
        //    }
        //}

        string[] find_ou = (await FindOrgUnitsAsync(new LdapSimpleRequestModel() { Query = user.OU, BaseFilters = [BasePath] })).ToArray();
        IQueryable<string> q = find_ou.Where(x => x.Equals(user.OU, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        if (q.Count() != 1)
        {
            msg = $"{msg}ошибка в имени 'OU' ({user.OU}). Результат поиска - {JsonConvert.SerializeObject(q.ToArray())}";
            _logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }
        user.OU = q.First();
        string name = $"{user.LastName} {user.FirstName} {user.PatronymicName}".Replace("  ", " "),
            dn = $"CN={name},{user.OU}";

        LdapMemberViewModel? find_user = (await FindMembersViewDataByQueryAsync([user.Login], [$"{findMembersPrefix}{BasePath}"], token)).FirstOrDefault(x => x.SAMAccountName.Equals(user.Login, StringComparison.OrdinalIgnoreCase));
        if (find_user is not null)
        {
            msg = $"{msg} TelegramId уже используется - {JsonConvert.SerializeObject(find_user)}";
            _logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }
        string password = $@"""{new PasswordGenerator().Generate()}""";

        LdapAttributeSet attributeSet = [];
        if (!string.IsNullOrWhiteSpace(user.TelegramId))
        {
            if (long.TryParse(user.TelegramId, out _))
            {

            }

            List<LdapMemberViewModel> chk_tg_user = await GetMembersViewDataByTelegramIdsAsync([user.TelegramId], token: token);
            if (chk_tg_user.Count != 0)
            {
                msg = $"{msg}ошибка в имени 'OU' ({user.OU}). Результат поиска - {JsonConvert.SerializeObject(chk_tg_user)}";
                _logger.LogError(msg);
                return ResponseBaseModel.CreateError(msg);

            }

            attributeSet.Add(new LdapAttribute(TelegramIdAttribute, user.TelegramId));
        }

        attributeSet.Add(new LdapAttribute("objectclass", "user"));
        attributeSet.Add(new LdapAttribute("userPrincipalName", user.Login.Contains("@") ? user.Login : $"{user.Login}@{_ad_opt.Host}"));
        attributeSet.Add(new LdapAttribute(UnicodePwdNameAttribute, Encoding.Unicode.GetBytes(password)));
        attributeSet.Add(new LdapAttribute(DisplayNameAttribute, name));
        attributeSet.Add(new LdapAttribute(CnNameAttribute, name));
        attributeSet.Add(new LdapAttribute(NameAttribute, name));
        attributeSet.Add(new LdapAttribute(SAMAccountNameAttribute, user.Login));
        attributeSet.Add(new LdapAttribute(PwdLastSetNameAttribute, "0"));
        attributeSet.Add(new LdapAttribute(UserAccountControlAttribute, "512"));

        attributeSet.Add(new LdapAttribute(MailNameAttribute, user.Email));
        attributeSet.Add(new LdapAttribute(GivenNameNameAttribute, user.FirstName));
        attributeSet.Add(new LdapAttribute(SnNameAttribute, user.LastName));
        attributeSet.Add(new LdapAttribute(DescriptionNameAttribute, user.Description));
        try
        {
            await ldap_conn!.AddAsync(new(dn, attributeSet), token);
            msg = $"В ad создана учётная запись `{user.Login}` ({JsonConvert.SerializeObject(new { name, dn })})";
            _logger.LogWarning(msg);
        }
        catch (LdapException ex)
        {
            msg = $"Ошибка создания пользователя ad/ldap: {ex.Message}. {ex.LdapErrorMessage}";
            _logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        //await _email.SendEmailAsync(user.Email, subject_mail, message_template.Replace("[accountname]", user.Login, StringComparison.OrdinalIgnoreCase).Replace("[password]", $"<strong>{password[1..^1]}</strong>", StringComparison.OrdinalIgnoreCase));

        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RenewPasswordAsync(string sAMAccountName, CancellationToken token = default)
    {
        LdapMemberViewModel? find_user = (await FindMembersViewDataByQueryAsync(new[] { sAMAccountName }, [$"{findMembersPrefix}{BasePath}"], token)).FirstOrDefault(x => x.SAMAccountName.Equals(sAMAccountName, StringComparison.OrdinalIgnoreCase));
        if (find_user is null)
            return ResponseBaseModel.CreateError($"Пользователь '{sAMAccountName}' не найден");

        string password = $@"""{new PasswordGenerator().Generate()}""";

        LdapModification[] modification = [
            new(LdapModification.Replace, new LdapAttribute(UnicodePwdNameAttribute, Encoding.Unicode.GetBytes(password))),
            new(LdapModification.Replace, new LdapAttribute(UserAccountControlAttribute, "512")),
            new(LdapModification.Replace, new LdapAttribute(PwdLastSetNameAttribute, "0"))
            ];
        await ldap_conn!.ModifyAsync(find_user.DistinguishedName, modification);

        string msg = "Новый пароль для вашей учётной записи";
        //await _email.SendEmailAsync(find_user.Email, msg, Properties.Resources.create_user_mail.Replace("Вам была заведена учетная запись", "Вам был сброшен пароль учетной записи").Replace("[accountname]", find_user.SAMAccountName).Replace("[password]", $"<strong>{password[1..^1]}</strong>"));

        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> FindOrgUnitsAsync(LdapSimpleRequestModel req, CancellationToken token = default)
    {
        if (req.BaseFilters?.Any(x => !string.IsNullOrWhiteSpace(x)) != true)
            return [];

        string searchFilter = "(objectClass=organizationalUnit)";
        if (!string.IsNullOrWhiteSpace(req.Query))
            searchFilter = $"(&(|({NameAttribute}=*{req.Query}*)({DistinguishedNameAttribute}=*{req.Query}*)({DescriptionNameAttribute}=*{req.Query}*)){searchFilter})";
        List<string> result = [];
        foreach (string bn in req.BaseFilters)
        {
            LdapSearchQueue searchQueue = await ldap_conn!.SearchAsync(bn, LdapConnection.ScopeSub, searchFilter, [], false, null as LdapSearchQueue, token) ?? throw new Exception("error {6664047D-F7E4-4C06-9345-BCD0DAB75EFB}");

            LdapMessage message;
            while ((message = searchQueue.GetResponse()) != null)
            {
                if (message is LdapSearchResult searchResult)
                {
                    LdapEntry user_entry = searchResult.Entry;
                    result.Add(user_entry.Dn);
                }
            }

            try
            {
                searchQueue = await ldap_conn!.SearchAsync(req.Query, LdapConnection.ScopeBase, "", [], false, null as LdapSearchQueue, token) ?? throw new Exception("error {43A95D92-3536-4762-87E3-816BBE39C72C}");
                while ((message = searchQueue.GetResponse()) != null)
                {
                    if (message is LdapSearchResult searchResult)
                    {
                        LdapEntry user_entry = searchResult.Entry;
                        result.Add(user_entry.Dn);
                    }
                }
            }
            finally
            {

            }
        }
        return result;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> FindMembersViewDataByQueryAsync(IEnumerable<string> queries, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        string query;
        if (queries.Count() > 1)
            query = $"(|{string.Join("", queries.Select(q => $"(|({CnNameAttribute}=*{GlobalTools.LdapEscapeValue(q)}*)({MailNameAttribute}=*{GlobalTools.LdapEscapeValue(q)}*)({SAMAccountNameAttribute}=*{GlobalTools.LdapEscapeValue(q)}*))"))})";
        else
        {
            string q = queries.First();
            query = $"(|({CnNameAttribute}=*{GlobalTools.LdapEscapeValue(q)}*)({MailNameAttribute}=*{GlobalTools.LdapEscapeValue(q)}*)({SAMAccountNameAttribute}=*{GlobalTools.LdapEscapeValue(q)}*))";
        }
        string searchFilter = string.Format("(&(objectClass=user)(objectClass=person){0})", query);

        base_filters = base_filters?.Any() == true
            ? base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct().ToArray()
            : _config_ldap_service.LdapBasedFiltersForFindUsers;

        List<LdapMemberViewModel> res = [.. await ReadMemberViewData(base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(), searchFilter, token: token)];
        return [.. res.DistinctBy(x => x.DistinguishedName)];
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetMembersViewDataByEmailsAsync(IEnumerable<string> emails, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!emails.Any())
            return [];

        string query, searchFilter;
        if (emails.Count() > 1)
        {
            query = string.Join("", emails.Select(x => $"({MailNameAttribute}={x})"));
            searchFilter = $"(&(objectClass=user)(objectClass=person)(|{query}))";
        }
        else
        {
            query = $"({MailNameAttribute}={emails.First()})";
            searchFilter = $"(&(objectClass=user)(objectClass=person){query})";
        }

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct()]
            : _config_ldap_service.LdapBasedFiltersForFindUsers;

        List<LdapMemberViewModel> res = [.. await ReadMemberViewData(base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(), searchFilter, token: token)];
        return [.. res.DistinctBy(x => x.DistinguishedName)];
    }

    /// <inheritdoc/>
    public async Task<List<LdapMemberViewModel>> GetMembersViewDataByTelegramIdsAsync(IEnumerable<string> ids, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!ids.Any())
            return [];

        string query, searchFilter;
        if (ids.Count() > 1)
        {
            query = string.Join("", ids.Select(x => $"({TelegramIdAttribute}={x})"));
            searchFilter = $"(&(objectClass=user)(objectClass=person)(|{query}))";
        }
        else
        {
            query = $"({TelegramIdAttribute}={ids.First()})";
            searchFilter = $"(&(objectClass=user)(objectClass=person){query})";
        }

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct()]
            : _config_ldap_service.LdapBasedFiltersForFindUsers;

        List<LdapMemberViewModel> res = [.. await ReadMemberViewData(base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(), searchFilter, token: token)];
        return [.. res.DistinctBy(x => x.DistinguishedName)];
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetMembersViewDataBySamAccountNamesAsync(IEnumerable<string> samaccounts_names, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!samaccounts_names.Any())
            return [];

        string query, searchFilter;
        if (samaccounts_names.Count() > 1)
        {
            query = string.Join("", samaccounts_names.Select(x => $"({SAMAccountNameAttribute}={x})"));
            searchFilter = $"(&(objectClass=user)(objectClass=person)(|{query}))";
        }
        else
        {
            query = $"({SAMAccountNameAttribute}={samaccounts_names.First()})";
            searchFilter = $"(&(objectClass=user)(objectClass=person){query})";
        }

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct()]
            : _config_ldap_service.LdapBasedFiltersForFindUsers;

        List<LdapMemberViewModel> res = [.. await ReadMemberViewData(base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(), searchFilter, true, token)];
        return [.. res.DistinctBy(x => x.DistinguishedName)];
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetTelegramIdForUserAsync(string user_dn, string telegram_id, CancellationToken token = default)
    {
        ResponseBaseModel res = new();
        string msg;
        LdapMemberViewModel? find_user_by_telegram_id = (await GetMembersViewDataByTelegramIdsAsync([telegram_id], token: token)).FirstOrDefault();
        if (find_user_by_telegram_id is not null)
        {
            if (find_user_by_telegram_id.DistinguishedName.Equals(user_dn, StringComparison.OrdinalIgnoreCase))
            {
                msg = $"Telegram ID и так уже установлен пользователю: {user_dn}.";
                _logger.LogWarning(msg);
                res.AddInfo(msg);
                return res;
            }
            msg = $"Telegram ID уже установлен пользователю: {find_user_by_telegram_id.SAMAccountName}. Его нельзя установить новому пользователю!";
            _logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        LdapModification[] modUser = [new(LdapModification.Replace, new LdapAttribute(TelegramIdAttribute, telegram_id))];

        try
        {
            await ldap_conn!.ModifyAsync(user_dn, modUser, token);
            res.AddSuccess("Telegram ID установлен");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка уровня novell/ldap:ad");
            res.AddError($"Ошибка уровня novell/ldap/ad: {ex.Message}");
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetMembersViewOfGroupAsync(string group_dn, CancellationToken token = default)
    {
        string searchFilter = string.Format($"(&(objectClass=user)(objectClass=person)({MemberOfNameAttribute}={{0}}))", group_dn);
        return [.. (await ReadMemberViewData([$"{findMembersPrefix}{BasePath}"], searchFilter, token: token)).DistinctBy(x => x.DistinguishedName)];
    }
    /// <summary>
    /// LdapMemberViewRecModel
    /// </summary>
    /// <param name="DistinguishedName">Distinguished name</param>
    /// <param name="ComonName">Основное имя</param>
    /// <param name="SAMAccountName">Логин</param>
    /// <param name="Email">Email</param>
    /// <param name="Description">Описание</param>
    /// <param name="IsDasabled">Отключён?</param>
    /// <param name="DisplayName">Отображаемое имя</param>
    /// <param name="PwdLastSet">Дата последнего изменения пароля</param>
    /// <param name="DoesntExpire">Пароль не может быть просрочен</param>
    /// <param name="Expired">Пароль просрочен?</param>
    /// <param name="PwdExpired">Окончание срока действия пароля</param>
    /// <param name="LastLogon">Последний вход</param>
    /// <param name="MemberOfGroups">Группы пользователя</param>
    record LdapMemberViewRecModel(string DistinguishedName, string ComonName, string SAMAccountName, string? Email, string? Description, bool IsDasabled, string? DisplayName, string? PwdLastSet, bool DoesntExpire, bool Expired, string PwdExpired, string LastLogon, IEnumerable<string>? MemberOfGroups, CancellationToken token = default)
    {
        public static implicit operator LdapMemberViewRecModel(LdapEntry entry)
        {
            LdapAttributeSet entryAttrs = entry.GetAttributeSet();

            entryAttrs.TryGetValue(msDS_UserAccountControlComputedNameAttribute, out var uaccv);
            bool pe = uint.TryParse(uaccv?.StringValue, out uint uacc) && (uacc & PASSWORD_EXPIRED) != 0;

            entryAttrs.TryGetValue(msDS_UserPasswordExpiryTimeComputedNameAttribute, out LdapAttribute? upev);
            string ped = string.Empty;
            if (long.TryParse(upev?.StringValue, out long upe) && upe != default && upe != long.MaxValue)
                ped = DateTime.FromFileTime(upe).ToString(GlobalStaticConstants._DT_FORMAT);

            entryAttrs.TryGetValue(CnNameAttribute, out LdapAttribute? cn);
            entryAttrs.TryGetValue(SAMAccountNameAttribute, out LdapAttribute? name);
            entryAttrs.TryGetValue(DisplayNameAttribute, out LdapAttribute? dname);
            entryAttrs.TryGetValue(MailNameAttribute, out LdapAttribute? mail);
            entryAttrs.TryGetValue(DescriptionNameAttribute, out LdapAttribute? description);
            entryAttrs.TryGetValue(UserAccountControlAttribute, out var control);
            entryAttrs.TryGetValue(PwdLastSetNameAttribute, out var pwdLastSetValue);

            string? pls = pwdLastSetValue?.StringValue;
            if (!string.IsNullOrWhiteSpace(pls))
                pls = DateTime.FromFileTime(long.Parse(pls)).ToString(GlobalStaticConstants._DT_FORMAT);

            entryAttrs.TryGetValue(LastLogonTimestampNameAttribute, out LdapAttribute? lastLogonTimestampValue);
            entryAttrs.TryGetValue(LastLogonNameAttribute, out LdapAttribute? lastLogonValue);
            long max = Math.Max(long.Parse(lastLogonTimestampValue?.StringValue ?? "0"), long.Parse(lastLogonValue?.StringValue ?? "0"));
            string ll = max == 0 ? string.Empty : DateTime.FromFileTime(max).ToString(GlobalStaticConstants._DT_FORMAT);

            bool disabled = false;
            bool notExpired = false;

            if (uint.TryParse(control?.StringValue, out uint value))
            {
                disabled = (value & ACCOUNTDISABLE) != 0;
                notExpired = (value & DONT_EXPIRE_PASSWORD) != 0;
            }

            if (cn is null || name is null)
                throw new Exception("ldap read error {0E2697A0-9735-4428-B3FD-6D6D36CF0C3E}");
            entryAttrs.TryGetValue(MemberOfNameAttribute, out LdapAttribute? memberOf_attr_values);

            return new LdapMemberViewRecModel(DistinguishedName: entry.Dn, ComonName: cn.StringValue, SAMAccountName: name.StringValue,
                Email: mail?.StringValue, Description: description?.StringValue, IsDasabled: disabled, DisplayName: dname?.StringValue,
              PwdLastSet: pls, DoesntExpire: notExpired, Expired: pe, PwdExpired: ped, LastLogon: ll,
             MemberOfGroups: memberOf_attr_values?.StringValueArray);
        }

        public static explicit operator LdapMemberViewModel(LdapMemberViewRecModel v)
        => new(distinguished_name: v.DistinguishedName, sam_account_name: v.SAMAccountName, display_name: v.DisplayName, common_name: v.ComonName,
            email: v.Email, is_disabled: v.IsDasabled, description: v.Description, pwd_last_set: v.PwdLastSet, doesnt_expire: v.DoesntExpire, expired: v.Expired,
            pwd_expired: v.PwdExpired, last_logon: v.LastLogon, memberOf: v.MemberOfGroups);
    }

    private async Task<List<LdapMemberViewModel>> ReadMemberViewData(IEnumerable<string> searchBase, string searchFilter, bool include_memer_of_groups = false, CancellationToken token = default)
    {
        List<LdapMemberViewModel> res = [];
        List<string> attributes_names = [CnNameAttribute,
            PwdLastSetNameAttribute,
            MailNameAttribute,
            LastLogonTimestampNameAttribute,
            DescriptionNameAttribute,
            LastLogonNameAttribute,
            SAMAccountNameAttribute,
            LockoutTimeNameAttribute,
            DisplayNameAttribute,
            //WhenChangedNameAttribute,
            //WhenCreatedNameAttribute,
            msDS_UserPasswordExpiryTimeComputedNameAttribute,
            UserAccountControlAttribute,
            msDS_UserAccountControlComputedNameAttribute];

        if (include_memer_of_groups)
            attributes_names.Add(MemberOfNameAttribute);

        foreach (string sb in searchBase)
        {
            try
            {
                SearchOptions so = new(sb, LdapConnection.ScopeSub, searchFilter, [.. attributes_names]);

                List<LdapMemberViewRecModel> raw = await ldap_conn.SearchUsingSimplePagingAsync<LdapMemberViewRecModel>(e => e, so, 100, cancellationToken: token);
                res.AddRange([.. raw.Select(x => (LdapMemberViewModel)x)]);
            }
            catch (LdapReferralException) { }// так надо
            catch (Exception ex)
            {
                _logger.LogError(ex, "read user/ldap data error E6C53545-3034-414E-A2FF-F0E2752E511B");
            }
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetDisableStateUserAsync(string sAMAccountName, bool setUserIsDisabled, string? newOuDistinguishedName, IEnumerable<string>? base_filters_users = null, CancellationToken token = default)
    {
        ResponseBaseModel res = new();
        string searchFilter = string.Format("(&(objectClass=user)(objectClass=person)({1}={0}))", sAMAccountName, SAMAccountNameAttribute);
        ILdapSearchResults result = await ldap_conn!.SearchAsync(BasePath, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute, SAMAccountNameAttribute, UserAccountControlAttribute], false
, token);
        LdapEntry? ldap_entry = await result.NextAsync(token);
        string msg;
        if (ldap_entry is null)
        {
            msg = $"Пользователь '{sAMAccountName}' не найден: {searchFilter}";
            res.AddError(msg);
            _logger.LogError(msg);
            return res;
        }
        string distinguishedName = ldap_entry.Dn;
        res.AddInfo($"Пользователь ldap:ad найден: {distinguishedName}");
        LdapAttributeSet attr_set = ldap_entry.GetAttributeSet();
        attr_set.TryGetValue(UserAccountControlAttribute, out LdapAttribute? control);
        bool is_disabled = uint.TryParse(control?.StringValue, out uint value) && ((value & ACCOUNTDISABLE) != 0);
        (bool is_disabled, string dn) user = (is_disabled, dn: distinguishedName);
        if (user.is_disabled == setUserIsDisabled)
            res.AddWarning($"Пользователь уже '{(!setUserIsDisabled ? "включён" : "отключён")}'.");
        else
        {
            LdapModification[] modGroup = new LdapModification[1];
            uint acc_val = setUserIsDisabled
                ? (value | ACCOUNTDISABLE)
                : (value & ~ACCOUNTDISABLE);

            LdapAttribute member = new(UserAccountControlAttribute, acc_val.ToString());
            modGroup[0] = new LdapModification(LdapModification.Replace, member);
            try
            {
                await ldap_conn!.ModifyAsync(user.dn, modGroup, token);
                res.AddSuccess($"Пользователю установлен атрибут [{UserAccountControlAttribute}:{acc_val}] ({(setUserIsDisabled ? "вык" : "вкл")})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка уровня novell/ldap:ad");
                res.AddError($"Ошибка уровня novell/ldap/ad: {ex.Message}");
            }
        }

        searchFilter = string.Format("(&(objectClass=organizationalUnit)({1}={0}))", newOuDistinguishedName, DistinguishedNameAttribute);
        result = await ldap_conn!.SearchAsync(BasePath, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute], false
, token);
        ldap_entry = await result.NextAsync(token);
        if (ldap_entry is null)
        {
            msg = $"OU (OrganizationalUnit) '{newOuDistinguishedName}' не найден: {searchFilter}";
            res.AddError(msg);
            _logger.LogError(msg);
            return res;
        }
        distinguishedName = ldap_entry.Dn;

        if (user.dn == $"CN={GlobalTools.GetCnAttrFromLdap(user.dn)},{distinguishedName}")
        {
            res.AddInfo("Перемещение в новую OU объекту не требуется");
            return res;
        }

        LdapModifyDnRequest dn_request = new(user.dn, $"CN={GlobalTools.GetCnAttrFromLdap(user.dn)}", distinguishedName, true, null);
        try
        {
            LdapMessageQueue ql = await ldap_conn.SendRequestAsync(dn_request, null, token);
            LdapMessage gr = ql.GetResponse();
            if (gr is LdapResponse lr)
            {
                if (!string.IsNullOrWhiteSpace(lr.ErrorMessage))
                {
                    msg = $"Во время попытки перенести пользователя в [OU:{distinguishedName}] произошла ошибка уровня novell/ldap: {lr.ErrorMessage}";
                    res.AddError(msg);
                    _logger.LogError(msg);
                }
                else
                {
                    msg = $"Пользователь перенесён в OU: {distinguishedName}";
                    res.AddSuccess(msg);
                    _logger.LogInformation(msg);
                }
            }
            else
            {
                msg = $"Пользователь перенесён в OU: {distinguishedName}";
                res.AddSuccess(msg);
                _logger.LogInformation(msg);
            }
        }
        catch (Exception ex)
        {
            res.AddError($"Ошибка перемещения пользователя в OU: {distinguishedName}.\n{ex.Message}");
            _logger.LogError(ex, $"Ошибка перемещения пользователя в OU: {distinguishedName}.");
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> FindMembersOfEmailsAsync(IEnumerable<string> emails, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        List<LdapMemberViewModel> res = [];
        if (emails?.Any() != true)
            return res;

        emails = [.. emails.Where(x => MailAddress.TryCreate(x, out _))];

        base_filters = base_filters?.Any() == true
           ? [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct()]
           : _config_ldap_service.LdapBasedFiltersForFindUsers;

        foreach (string[] part_email in emails.Chunk(100))
        {
            string query_find_by_emails = string.Join(string.Empty, part_email.Select(x => $"(mail={x})"));
            string searchFilter = string.Format("(&(objectClass=user)(objectClass=person)(|{0}))", query_find_by_emails);

            IEnumerable<LdapMemberViewModel> sr = await ReadMemberViewData(base_filters, searchFilter, true, token);
            if (sr.Any())
                res.AddRange(sr);
        }
        return res;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMinimalModel>> FindGroupsDNByCNAsync(IEnumerable<string> queries_cn, IEnumerable<string> base_filters, bool is_scope_sub = true, CancellationToken token = default)
    {
        if (!queries_cn.Any())
            return [];

        List<string> res = [];
        List<LdapMinimalModel> result = [];
        string query, searchFilter;
        if (queries_cn.Count() > 1)
        {
            query = string.Join("", queries_cn.Select(x => $"({CnNameAttribute}={x})"));
            searchFilter = $"(&(objectClass=group)(|{query}))";
        }
        else
        {
            query = $"({CnNameAttribute}={queries_cn.First()})";
            searchFilter = $"(&(objectClass=group){query})";
        }

        base_filters = [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()];

        foreach (string bn in base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
            foreach (LdapMinimalModel r in await ReadGroupData(bn, searchFilter, token))
                result.Add(r);

        return result;
    }

    async Task<List<LdapMinimalModel>> ReadGroupData(string searchBase, string searchFilter, CancellationToken token = default)
    {
        List<LdapMinimalModel> res = [];

        try
        {
            SearchOptions so = new(searchBase, LdapConnection.ScopeSub, searchFilter, [CnNameAttribute, SAMAccountNameAttribute]);

            List<LdapMinimalRecModel> raw = await ldap_conn!.SearchUsingSimplePagingAsync<LdapMinimalRecModel>(e => e, so, 100, cancellationToken: token);
            res.AddRange([.. raw.Select(x => (LdapMinimalModel)x)]);
        }
        catch (LdapReferralException) { }// так надо

        return res;
    }
    record LdapMinimalRecModel(string DistinguishedName, string CommonName, string SAMAccountName)
    {
        public static implicit operator LdapMinimalRecModel(LdapEntry entry)
        {
            LdapAttributeSet entryAttrs = entry.GetAttributeSet();

            string sam_account_name, cn;

            entryAttrs.TryGetValue(CnNameAttribute, out LdapAttribute? la);
            cn = la?.StringValue ?? string.Empty;

            entryAttrs.TryGetValue(SAMAccountNameAttribute, out la);
            sam_account_name = la?.StringValue ?? string.Empty;

            return new LdapMinimalRecModel(entry.Dn, cn, sam_account_name);
        }

        public static explicit operator LdapMinimalModel(LdapMinimalRecModel v)
        => new(v.DistinguishedName, v.SAMAccountName, v.CommonName);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetGroupsDNForUserAsync(string user_dn, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        string searchFilter = string.Format("(&(objectClass=group)(member={0}))", user_dn);

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Distinct()]
            : new string[] { BasePath };

        List<string> result = [];

        foreach (string bn in base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
        {
            LdapSearchQueue searchQueue = await ldap_conn!.SearchAsync(bn, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute], false, null as LdapSearchQueue
, token);

            LdapMessage message;
            while ((message = searchQueue.GetResponse()) != null)
            {
                if (message is LdapSearchResult searchResult)
                {
                    LdapEntry group_entry = searchResult.Entry;
                    result.Add(group_entry.Dn);
                }
            }
        }
        return result;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> FindGroupsDNByCodeTemplateAsync(string code_template, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        string searchFilter = string.Format($"(&(objectClass=group)(|({CnNameAttribute}={{0}}_*)({CnNameAttribute}={{0}}-*)({CnNameAttribute}={{0}})))", code_template);

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Distinct()]
            : new string[] { BasePath };

        List<string> result = [];
        foreach (string bn in base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
        {
            LdapSearchQueue searchQueue = await ldap_conn!.SearchAsync(bn, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute], false, null as LdapSearchQueue
, token);

            LdapMessage message;
            while ((message = searchQueue.GetResponse()) != null)
            {
                if (message is LdapSearchResult searchResult)
                {
                    LdapEntry group_entry = searchResult.Entry;
                    result.Add(group_entry.Dn);
                }
            }
        }
        return result;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> FindGroupsDNByQueryCNAsync(IEnumerable<string> queries_cn, FindTextModesEnum mode, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (base_filters?.Any(x => !string.IsNullOrWhiteSpace(x)) != true)
            return [];

        base_filters = [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()];
        queries_cn = queries_cn.Any(x => x.Trim() == "*")
            ? []
            : [.. queries_cn.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()];

        IEnumerable<string> pre_q = [.. queries_cn
            .Select(x =>
            {
                return mode switch
                {
                    FindTextModesEnum.Equal => $"(cn={x})",
                    FindTextModesEnum.Contains => $"(cn=*{x}*)",
                    FindTextModesEnum.NotContains => $"!(cn=*{x}*)",
                    FindTextModesEnum.BeginsWith => $"(cn={x}*)",
                    FindTextModesEnum.NotEqual => $"!(cn={x})",
                    FindTextModesEnum.EndsWith => $"(cn=*{x})",
                    _ => "(1=0)"
                };
            })];

        string q = pre_q.Any() ? $"(|{string.Join("", pre_q)})" : "";
        string searchFilter = string.IsNullOrWhiteSpace(q) ? "(objectClass=group)" : string.Format("(&(objectClass=group){0})", q);

        List<string> result = [];
        foreach (string bn in base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
        {
            LdapSearchQueue searchQueue = await ldap_conn!.SearchAsync(bn, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute], false, null as LdapSearchQueue
, token);

            LdapMessage message;
            while ((message = searchQueue.GetResponse()) != null)
            {
                if (message is LdapSearchResult searchResult)
                {
                    LdapEntry group_entry = searchResult.Entry;
                    result.Add(group_entry.Dn);
                }
            }
        }
        return result;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapGroupViewModel>> GetGroupsBySAMAccountNamesAsync(IEnumerable<string> groups_sam_account_names, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!groups_sam_account_names.Any())
            return [];

        string searchFilter, query;

        if (groups_sam_account_names.Count() > 1)
        {
            query = string.Join("", groups_sam_account_names.Select(x => $"({SAMAccountNameAttribute}={x})"));
            searchFilter = $"(&(objectClass=top)(objectClass=group)(|{query}))";
        }
        else
        {
            query = $"{SAMAccountNameAttribute}={groups_sam_account_names.First()}";
            searchFilter = $"(&(objectClass=top)(objectClass=group)({query}))";
        }
        base_filters = base_filters?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct();
        base_filters = base_filters?.Any() == true
            ? base_filters.ToArray()
            : [BasePath];

        return await LdapGroupRead(searchFilter, base_filters, token);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapGroupViewModel>> GetGroupsByDistinguishedNamesAsync(IEnumerable<string> groups_dns, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!groups_dns.Any())
            return [];

        string searchFilter, query;

        if (groups_dns.Count() > 1)
        {
            query = string.Join("", groups_dns.Select(x => $"({DistinguishedNameAttribute}={x})"));
            searchFilter = $"(&(objectClass=top)(objectClass=group)(|{query}))";
        }
        else
        {
            query = $"{DistinguishedNameAttribute}={groups_dns.First()}";
            searchFilter = $"(&(objectClass=top)(objectClass=group)({query}))";
        }
        base_filters = base_filters?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct();
        base_filters = base_filters?.Any() == true
            ? [.. base_filters]
            : _config_ldap_service.LdapBasedFiltersForFindGroups;

        return await LdapGroupRead(searchFilter, base_filters, token);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapGroupViewModel>> GetGroupsByParentDistinguishedNamesAsync(IEnumerable<string> groups_dns, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!groups_dns.Any())
            return [];

        string searchFilter, query;

        if (groups_dns.Count() > 1)
        {
            query = string.Join("", groups_dns.Select(x => $"({MemberOfNameAttribute}={x})"));
            searchFilter = $"(&(objectClass=top)(objectClass=group)(|{query}))";
        }
        else
        {
            query = $"{MemberOfNameAttribute}={groups_dns.First()}";
            searchFilter = $"(&(objectClass=top)(objectClass=group)({query}))";
        }
        base_filters = base_filters?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct();
        base_filters = base_filters?.Any() == true
            ? base_filters.ToArray()
            : [BasePath];

        return await LdapGroupRead(searchFilter, base_filters, token);
    }

    async Task<List<LdapGroupViewModel>> LdapGroupRead(string searchFilter, IEnumerable<string> base_filters, CancellationToken token = default)
    {
        List<LdapGroupViewModel> res = [];

        foreach (string bn in base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
        {
            try
            {
                SearchOptions so = new(bn, LdapConnection.ScopeSub, searchFilter, null);
                List<LdapGroupViewRecModel> raw = await ldap_conn.SearchUsingSimplePagingAsync<LdapGroupViewRecModel>(e => e, so, 100, cancellationToken: token);
                res.AddRange([.. raw.Select(x => (LdapGroupViewModel)x)]);
            }
            catch (LdapReferralException) { }// так надо
            catch (LdapException ex)
            {
                if (ex.LdapErrorMessage.Contains("problem 2001 (NO_OBJECT)", StringComparison.OrdinalIgnoreCase))
                    continue;

                string msg = $"При выполнении запроса к LDAP/AD (searchBase:{bn}) возникло исключение '{ex.GetType().Name}' ({ex.LdapErrorMessage} // {ex.MatchedDn}): {ex.Message}\n\n{searchFilter}";

                if (ex.InnerException is not null)
                    msg += $"\n\n\t\tINNER EXEPTION:{ex.InnerException.Message}";

                _logger.LogError(ex, msg);
            }
            catch (Exception ex)
            {
                string msg = $"При выполнении запроса к LDAP/AD (searchBase:{bn}) возникло исключение '{ex.GetType().Name}': {ex.Message}\n\n{searchFilter}";

                if (ex.InnerException is not null)
                    msg += $"\n\n\t\tINNER EXEPTION:{ex.InnerException.Message}";

                _logger.LogError(ex, msg);
            }
        }

        return res;
    }
    record LdapGroupViewRecModel(string Name, string SAMAccountName, string DistinguishedName, string CommonName, IEnumerable<string> MembersDn)
        : LdapMinimalRecModel(DistinguishedName: DistinguishedName, CommonName: CommonName, SAMAccountName: SAMAccountName)
    {
        public static implicit operator LdapGroupViewRecModel(LdapEntry entry)
        {
            List<string> members = [];
            string name_attr_value, sam_account_name, cn;
            LdapAttributeSet entryAttrs = entry.GetAttributeSet();

            entryAttrs.TryGetValue(NameAttribute, out LdapAttribute? la);
            name_attr_value = la?.StringValue ?? string.Empty;

            entryAttrs.TryGetValue(SAMAccountNameAttribute, out la);
            sam_account_name = la?.StringValue ?? string.Empty;

            entryAttrs.TryGetValue(CnNameAttribute, out la);
            cn = la?.StringValue ?? string.Empty;

            string[] members_keys = [.. entryAttrs.Keys.Where(x => x.Equals(MemberNameAttribute, StringComparison.OrdinalIgnoreCase) || x.StartsWith($"{MemberNameAttribute};", StringComparison.OrdinalIgnoreCase))];

            foreach (string mk in members_keys)
            {
                entryAttrs.TryGetValue(mk, out la);
                if (la is not null)
                    members.AddRange(la.StringValueArray);
            }

            return new LdapGroupViewRecModel(Name: name_attr_value, SAMAccountName: sam_account_name, DistinguishedName: entry.Dn, CommonName: cn, MembersDn: members);
        }

        public static explicit operator LdapGroupViewModel(LdapGroupViewRecModel v) => new(distinguished_name: v.DistinguishedName, sam_account_name: v.SAMAccountName, common_name: v.CommonName, name: v.Name) { MembersDn = v.MembersDn };
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetMembersBySAMAccountNamesAsync(IEnumerable<string> users_sam_account_names, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!users_sam_account_names.Any())
            return [];

        string query, searchFilter;

        if (users_sam_account_names.Count() > 1)
            query = $"(|{string.Join("", users_sam_account_names.Select(x => string.Format("({1}={0})", x, SAMAccountNameAttribute)))})";
        else
            query = $"({SAMAccountNameAttribute}={users_sam_account_names.First()})";

        searchFilter = string.Format("(&(objectClass=user)(objectClass=person){0})", query);

        base_filters = base_filters?.Any() == true
            ? base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct().ToArray()
            : [$"{findMembersPrefix}{BasePath}"];

        return await ReadMemberViewData(base_filters, searchFilter, true, token);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetUsersByDistinguishedNamesAsync(IEnumerable<string> users_dns, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        if (!users_dns.Any())
            return [];

        string query, searchFilter;

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct()]
            : _config_ldap_service.LdapBasedFiltersForFindUsers;

        IEnumerable<IEnumerable<string>> users_dns_parts = SplitUsersDns(users_dns);
        List<LdapMemberViewModel> res = [];
        foreach (IEnumerable<string> part_dns in users_dns_parts)
        {
            if (part_dns.Count() > 1)
                query = $"(|{string.Join("", part_dns.Select(x => string.Format("({1}={0})", GlobalTools.LdapEscapeValue(x), DistinguishedNameAttribute)))})";
            else
                query = $"({DistinguishedNameAttribute}={GlobalTools.LdapEscapeValue(part_dns.First())})";

            searchFilter = string.Format("(&(objectClass=user)(objectClass=person){0})", query);

            try
            {
                res.AddRange(await ReadMemberViewData(base_filters, searchFilter, true, token));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        return res;
    }

    static List<IEnumerable<string>> SplitUsersDns(IEnumerable<string> users_dns)
    {
        int part_size = 500;
        List<IEnumerable<string>> res = [];
        if (users_dns.Count() <= part_size)
        {
            res.Add(users_dns);
            return res;
        }

        res.Add(users_dns.Take(part_size));
        users_dns = [.. users_dns.Skip(part_size)];

        while (users_dns.Any())
        {
            res.Add(users_dns.Take(part_size));
            users_dns = [.. users_dns.Skip(part_size)];
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> FindMembersOfGroupsByQueryGroupNameAsync(IEnumerable<string> queries_for_groups_names, FindTextModesEnum mode, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        queries_for_groups_names = queries_for_groups_names.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => GlobalTools.LdapEscapeValue(x)).ToArray();
        if (!queries_for_groups_names.Any())
            return [];

        base_filters ??= [BasePath];
        IEnumerable<string> groups_dn = [.. await FindGroupsDNByQueryCNAsync(queries_for_groups_names, mode, base_filters, token)];

        if (!groups_dn.Any())
            return [];

        string searchFilter, q;
        if (groups_dn.Count() > 1)
            q = $"(|{string.Join("", groups_dn.Select(x => string.Format("({1}={0})", x, MemberOfNameAttribute)))})";
        else
            q = string.Format("({1}={0})", groups_dn.First(), MemberOfNameAttribute);

        searchFilter = string.Format("(&(objectClass=user)(objectClass=person){0})", q);

        return await ReadMemberViewData(base_filters, searchFilter, true, token);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapPersonBaseViewModel>> GetMetadataGroupsForUserAsync(string user_name, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        string query, searchFilter;
        query = $"({SAMAccountNameAttribute}={user_name})";
        searchFilter = string.Format("(&(objectClass=user)(objectClass=person){0})", query);

        string distinguishedName, sAMAccountName, displayName, cn;
        IEnumerable<string> memberOf_attr_values = [];
        LdapMessage message;
        LdapSearchQueue searchQueue;
        LdapAttributeSet attr_set;
        LdapEntry ldap_entry;
        List<LdapUserKeysPairWithGroupsModel> finded_users = [];

        searchQueue = await ldap_conn!.SearchAsync(BasePath, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute, SAMAccountNameAttribute, MemberOfNameAttribute, DisplayNameAttribute, CnNameAttribute], false, null as LdapSearchQueue
, token);

        while ((message = searchQueue.GetResponse()) != null && finded_users.Count < 2)
        {
            if (message is LdapSearchResult searchResult)
            {
                ldap_entry = searchResult.Entry;

                distinguishedName = ldap_entry.Dn;
                sAMAccountName = ldap_entry.Get(SAMAccountNameAttribute).StringValue;

                attr_set = ldap_entry.GetAttributeSet();

                attr_set.TryGetValue(MemberOfNameAttribute, out LdapAttribute? la);
                memberOf_attr_values = la?.StringValueArray ?? Enumerable.Empty<string>();

                attr_set.TryGetValue(DisplayNameAttribute, out la);
                displayName = la?.StringValue ?? "";

                attr_set.TryGetValue(CnNameAttribute, out la);
                cn = la?.StringValue ?? "";

                finded_users.Add(new LdapUserKeysPairWithGroupsModel(distinguished_name: distinguishedName, sam_account_name: sAMAccountName, display_name: displayName, common_name: cn, groups_dn: [])
                { DisplayName = displayName, SAMAccountName = sAMAccountName, GroupsDNs = [.. memberOf_attr_values] });
            }
        }


        if (finded_users.Count != 1)
            throw new Exception("finded_users.Count != 1. ошибка {CF43F253-939D-4B46-AE05-35942AB16555}");

        LdapUserKeysPairWithGroupsModel user = finded_users.First();

        if (!user.GroupsDNs.Any())
            return [];

        if (base_filters?.Any() != true)
            base_filters = [BasePath];

        List<LdapPersonBaseViewModel> res_groups = [];
        int part_size = 100;
        int part_count = 0;
        string[] part_groups = [.. user.GroupsDNs.Take(part_size)];

        while (part_groups.Length != 0)
        {
            if (part_groups.Length == 1)
            {
                query = $"{DistinguishedNameAttribute}={part_groups.First()}";
                searchFilter = $"(&(objectClass=top)(objectClass=group)({query}))";
            }
            else
            {
                query = string.Join("", part_groups.Select(x => $"({DistinguishedNameAttribute}={x})"));
                searchFilter = $"(&(objectClass=top)(objectClass=group)(|{query}))";
            }

            foreach (string bf in base_filters)
            {
                try
                {
                    SearchOptions so = new(bf, LdapConnection.ScopeSub, searchFilter, [DistinguishedNameAttribute, SAMAccountNameAttribute]);
                    List<LdapPersonBaseViewRecModel> raw = await ldap_conn.SearchUsingSimplePagingAsync<LdapPersonBaseViewRecModel>(e => e, so, 100);
                    res_groups.AddRange([.. raw.Select(x => (LdapPersonBaseViewModel)x)]);
                }
                catch (LdapReferralException) { }// так надо
            }

            part_count++;
            part_groups = [.. user.GroupsDNs.Skip(part_size * part_count).Take(part_size)];
        }
        return res_groups;
    }
    record LdapPersonBaseViewRecModel(string DistinguishedName, string SAMAccountName, string DisplayName, string CommonName)
    {
        public static implicit operator LdapPersonBaseViewRecModel(LdapEntry entry)
        {
            LdapAttributeSet entryAttrs = entry.GetAttributeSet();

            string sam_account_name, display_name, common_name;

            string distinguishedName = entry.Dn;

            entryAttrs.TryGetValue(SAMAccountNameAttribute, out LdapAttribute? la);
            sam_account_name = la?.StringValue ?? string.Empty;

            entryAttrs.TryGetValue(DisplayNameAttribute, out la);
            display_name = la?.StringValue ?? string.Empty;

            entryAttrs.TryGetValue(CnNameAttribute, out la);
            common_name = la?.StringValue ?? string.Empty;

            return new LdapPersonBaseViewRecModel(distinguishedName, sam_account_name, display_name, common_name);
        }

        public static explicit operator LdapPersonBaseViewModel(LdapPersonBaseViewRecModel v) => new(v.DistinguishedName, v.SAMAccountName, v.DisplayName, v.CommonName);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetMembersOfGroupsAsync(IEnumerable<string> groups_dn_memberOf, CancellationToken token = default)
    {
        List<LdapMemberViewModel> res = [];
        groups_dn_memberOf = [.. groups_dn_memberOf.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => GlobalTools.LdapEscapeValue(x))];
        if (!groups_dn_memberOf.Any())
            return res;

        string query_find_by_groups, searchFilter;
        foreach (string[] gp in groups_dn_memberOf.Chunk(100))
        {
            if (gp.Length > 1)
            {
                query_find_by_groups = string.Join(string.Empty, gp.Select(x => $"(memberOf={x})"));
                searchFilter = string.Format("(&(objectClass=user)(objectClass=person)(mail=*)(pwdLastSet=*)(|{0}))", query_find_by_groups);
            }
            else
            {
                query_find_by_groups = $"(memberOf={gp.First()})";
                searchFilter = string.Format("(&(objectClass=user)(objectClass=person)(mail=*)(pwdLastSet=*){0})", query_find_by_groups);
            }
            IEnumerable<LdapMemberViewModel> sr = await ReadMemberViewData(_config_ldap_service.LdapBasedFiltersForFindUsers, searchFilter, true, token);
            if (sr.Any())
                res.AddRange(sr);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapMemberViewModel>> GetAllEmailsAsync(IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        base_filters ??= _config_ldap_service.LdapBasedFiltersForFindUsers;
        string searchFilter = string.Format("(&(objectClass=user)(objectCategory=person)(mail=*)(pwdLastSet=*)(!(userAccountControl:{1}:={0})))", UacDontHumanFlags, LDAP_MATCHING_RULE_BIT_OR);

        return await ReadMemberViewData(base_filters, searchFilter, true, token);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LdapGroupViewModel>> FindGroupsAsync(string query, IEnumerable<string>? base_filters = null, CancellationToken token = default)
    {
        string searchFilter = string.Format("(&(objectClass=top)(objectClass=group)(|({1}=*{0}*)({2}=*{0}*)({3}=*{0}*)))", query, CnNameAttribute, SAMAccountNameAttribute, NameAttribute);

        base_filters = base_filters?.Any() == true
            ? [.. base_filters.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct()]
            : new string[] { BasePath };

        return await LdapGroupRead(searchFilter, base_filters, token);
    }

    /// <inheritdoc/>
    public async Task<LdapMembersViewsResponseModel> InjectMemberToGroupAsync(string user_dn, string group_dn, IEnumerable<string>? base_filters_users = null, IEnumerable<string>? base_filters_groups = null, CancellationToken token = default)
    {
        LdapUserAndGroupResultModel check = await CheckUserGroupPairAsync(new() { UserDN = user_dn, GroupDN = group_dn }, base_filters_users, base_filters_groups, token);
        LdapMembersViewsResponseModel res = new();
        string msg;
        if (check.GroupData is null || check.UserData is null)
        {
            msg = $"Ошибка добавления пользователя [{user_dn}] к группе [{group_dn}]: пользователь или группа не найдены {{40A7A6A2-9A36-479C-ADAA-B6EE0E540260}}";
            _logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        if (check.UserData.MemberOf?.Any(x => x.Equals(group_dn, StringComparison.OrdinalIgnoreCase)) == true)
        {
            msg = $"Пользователь `{check.UserData.SAMAccountName}` [{user_dn}] уже в группе `{check.GroupData.SAMAccountName}` [{group_dn}]. Добавление не требуется!";
            _logger.LogInformation(msg);
            res.AddInfo(msg);
            res.Members = (await GetMembersViewOfGroupAsync(group_dn, token)).OrderBy(x => x.DistinguishedName);
            return res;
        }

        // modifications for group
        LdapModification[] modGroup = new LdapModification[1];

        // Add modifications to modGroup
        LdapAttribute member = new(MemberNameAttribute, user_dn);
        modGroup[0] = new LdapModification(LdapModification.Add, member);

        try
        {
            await ldap_conn!.ModifyAsync(group_dn, modGroup, token);
            msg = $"Пользователь `{check.UserData.SAMAccountName}` [{user_dn}] добавлен в группу `{check.GroupData.SAMAccountName}` [{group_dn}]!";
            _logger.LogWarning(msg);
            res.AddSuccess(msg);
        }
        catch (Exception ex)
        {
            msg = $"Ошибка (уровня novell/ldap:ad) добавления пользователя `{check.UserData.SAMAccountName}` [{user_dn}] в группу `{check.GroupData.SAMAccountName}` [{group_dn}]";
            _logger.LogError(ex, msg);
            res.AddError($"{msg}: {ex.Message}");
        }

        res.Members = (await GetMembersViewOfGroupAsync(group_dn, token)).OrderBy(x => x.DistinguishedName);
        return res;
    }

    /// <inheritdoc/>
    public async Task<LdapMembersViewsResponseModel> KickMemberFromGroupAsync(string user_dn, string group_dn, IEnumerable<string>? base_filters_users = null, IEnumerable<string>? base_filters_groups = null, CancellationToken token = default)
    {
        LdapUserAndGroupResultModel check = await CheckUserGroupPairAsync(new() { UserDN = user_dn, GroupDN = group_dn }, base_filters_users, base_filters_groups, token);
        LdapMembersViewsResponseModel res = new();
        string msg;
        if (check.GroupData is null || check.UserData is null)
        {
            msg = $"Ошибка удаления пользователя [{user_dn}] из группы {group_dn} {{9C1FC82B-1225-43EF-8F93-335BF4D270E4}}: пользователь и/или группа не найдены - {JsonConvert.SerializeObject(check)}";
            _logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        if (check.UserData.MemberOf?.Any(x => x.Equals(group_dn, StringComparison.OrdinalIgnoreCase)) != true)
        {
            msg = $"Пользователя `{check.UserData.SAMAccountName}` [{user_dn}] нет в группе `{check.GroupData.SAMAccountName}` [{group_dn}]. Удаление не требуется!";
            _logger.LogInformation(msg);
            res.AddInfo(msg);
            res.Members = (await GetMembersViewOfGroupAsync(group_dn, token)).OrderBy(x => x.DistinguishedName);
            return res;
        }

        // modifications for group
        LdapModification[] modGroup = new LdapModification[1];

        // Add modifications to modGroup
        LdapAttribute member = new(MemberNameAttribute, user_dn);
        modGroup[0] = new LdapModification(LdapModification.Delete, member);

        try
        {
            await ldap_conn!.ModifyAsync(group_dn, modGroup, token);
            msg = $"Пользователь `{check.UserData.SAMAccountName}` [{user_dn}] удалён из группы {check.GroupData.SAMAccountName} [{group_dn}]!";
            _logger.LogWarning(msg);
            res.AddSuccess(msg);
        }
        catch (Exception ex)
        {
            msg = $"Ошибка (уровня novell/ldap:ad) удаления пользователя `{check.UserData.SAMAccountName}` [{user_dn}] из группы `{check.GroupData.SAMAccountName}` [{group_dn}]";
            _logger.LogError(ex, msg);
            res.AddError($"{msg}: {ex.Message}");
        }

        res.Members = await GetMembersViewOfGroupAsync(group_dn, token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<LdapUserAndGroupResultModel> CheckUserGroupPairAsync(LdapUserAndGroupModel inc, IEnumerable<string>? base_filters_users = null, IEnumerable<string>? base_filters_groups = null, CancellationToken token = default)
    {
        LdapUserAndGroupResultModel res = new();
        if (!string.IsNullOrWhiteSpace(inc.UserDN))
            res.UserData = (await GetUsersByDistinguishedNamesAsync([inc.UserDN], token: token)).FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(inc.GroupDN))
            res.GroupData = (await GetGroupsByDistinguishedNamesAsync([inc.GroupDN], token: token)).FirstOrDefault();

        return res;
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (ldap_conn?.Connected == true)
            ldap_conn.Disconnect();
    }
}