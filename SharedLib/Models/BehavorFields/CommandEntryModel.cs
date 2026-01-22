////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Команда (в виде стандартного элемента)
/// </summary>
public class CommandEntryModel : EntryAltDescriptionStandardModel
{
    /// <summary>
    /// Разрешение запуска/вызова без параметров
    /// </summary>
    public bool AllowCallWithoutParameters { get; set; } = false;
}