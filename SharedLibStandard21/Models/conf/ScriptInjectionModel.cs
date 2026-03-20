////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Collections.Generic;

namespace SharedLib;

/// <summary>
/// InjectionBaseModel
/// </summary>
public class InjectionBaseModel
{
    /// <summary>
    /// Ссылка к скрипту
    /// </summary>
    public string? Src { get; set; }

    /// <summary>
    /// Пути для инъекции ссылки
    /// </summary>
    /// <remarks>
    /// Если не указано, тогда инъекция для всех страниц.
    /// </remarks>
    public string[]? LocalPathsForInject { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, object>? Attributes { get; set; }
}