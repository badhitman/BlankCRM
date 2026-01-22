////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Остатки на складах
/// </summary>
[Index(nameof(RemainValue))]
public class WarehouseRemainsRusklimatModelDB:EntryStandardModel
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