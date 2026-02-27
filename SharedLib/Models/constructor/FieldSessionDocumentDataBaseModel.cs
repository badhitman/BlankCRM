////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Строки данных таблиц
/// </summary>
public class FieldSessionDocumentDataBaseModel
{
    /// <summary>
    /// Сессия опроса/анкеты
    /// </summary>
    public int SessionId { get; set; }

    /// <summary>
    /// Связь формы со страницей опроса
    /// </summary>
    public int JoinFormId { get; set; }
}

/// <summary>
/// GetCurrentMainProjectRequestModel
/// </summary>
public class GetCurrentMainProjectRequestModel
{
    /// <inheritdoc/>
    public required string UserIdentityId { get; set; }

    /// <inheritdoc/>
    public string? ContextName { get; set; }
}