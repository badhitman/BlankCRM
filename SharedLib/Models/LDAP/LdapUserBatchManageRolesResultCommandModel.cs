////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Отчёт по пользователю и связанных с ним команд над связями с ролями
/// </summary>
public class LdapUserBatchManageRolesResultCommandModel
{
    /// <summary>
    /// Email/login пользователя
    /// </summary>
    public string UserEmail { get; set; } = string.Empty;

    /// <summary>
    /// Отчёт по работам для пользователя
    /// </summary>
    public IEnumerable<LdapJobBatchManageRolesResultCommandModel> Jobs { get; set; } = [];
}