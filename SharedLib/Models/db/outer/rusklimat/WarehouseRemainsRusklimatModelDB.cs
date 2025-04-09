////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Остатки на складах
/// </summary>
public class WarehouseRemainsRusklimatModelDB:EntryModel
{
    /// <summary>
    /// Parent
    /// </summary>
    public RemainsRusklimatModelDB? Parent { get; set; }
    /// <summary>
    /// Parent
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// Остаток
    /// </summary>
    public string? RemainValue { get; set; }
}