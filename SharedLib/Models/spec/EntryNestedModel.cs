////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Простейший вещественный тип вложенной/древовидной структуры
/// </summary>
public class EntryNestedModel : EntryStandardModel
{
    /// <summary>
    /// Вложенные (дочерние) объекты
    /// </summary>
    public IEnumerable<EntryStandardModel> Childs { get; set; } = [];
}