////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Результат работы команды
/// </summary>
public class LdapJobBatchManageRolesResultCommandModel : ResponseBaseModel
{
    /// <summary>
    /// Имя роли
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
}
