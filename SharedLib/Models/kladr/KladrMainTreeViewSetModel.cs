////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Настройки отображения навигационного дерева КЛАДР
/// </summary>
public class KladrMainTreeViewSetModel
{
    /// <summary>
    /// Отображаемые поля
    /// </summary>
    public IReadOnlyCollection<string>? SelectedFieldsView { get; set; }
}