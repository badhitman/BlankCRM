////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пользователь с Email, описанием и признаком вкл/выкл
/// </summary>
public class LdapMemberViewModel(string distinguished_name, string sam_account_name, string? display_name, string common_name, string? email, bool is_disabled, string? description, string? pwd_last_set, bool doesnt_expire, bool expired, string pwd_expired, string last_logon, IEnumerable<string>? memberOf = null) : LdapPersonBaseViewModel(distinguished_name, sam_account_name, display_name, common_name)
{
    /// <summary>
    /// Пароль без срока давности
    /// </summary>
    public bool DoesntExpire { get; set; } = doesnt_expire;

    /// <summary>
    /// Пароль просрочен
    /// </summary>
    public bool Expired { get; set; } = expired;

    /// <summary>
    /// Пароль просрочен
    /// </summary>
    public string PasswordExpired { get; set; } = pwd_expired;

    /// <summary>
    /// Последний вход (Timestamp)
    /// </summary>
    public string? LastLogonTimestamp { get; set; } = last_logon;

    /// <summary>
    /// Последний раз менялся пароль (Timestamp)
    /// </summary>
    public string? PwdLastSet { get; set; } = pwd_last_set;

    /// <summary>
    /// E-mail участника групп
    /// </summary>
    public string? Email { get; set; } = email;

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; } = description;

    /// <summary>
    /// Удалён
    /// </summary>
    public bool IsDisabled { get; set; } = is_disabled;

    /// <summary>
    /// Группы, в которых пользователь учувствует
    /// </summary>
    public IEnumerable<string>? MemberOf { get; set; } = memberOf;
}