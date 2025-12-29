////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InjectionBaseModel
/// </summary>
public class InjectionBaseModel()
{
    /// <summary>
    /// Ссылка к скрипту
    /// </summary>
    public required string Src { get; set; }

    /// <summary>
    /// Пути для инъекции ссылки
    /// </summary>
    /// <remarks>
    /// Если не указано, тогда инъекция для всех страниц.
    /// </remarks>
    public string[]? LocalPathsForInject { get; set; }
}