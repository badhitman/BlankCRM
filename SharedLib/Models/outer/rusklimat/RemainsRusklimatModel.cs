////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// остаток товара на складе
/// </summary>
public class RemainsRusklimatModel
{
    /// <summary>
    /// сумма остатков по всем складам
    /// </summary>
    public string? Total { get; set; }

    /// <summary>
    /// детализация остатков по складам в формате "название склада": "остаток на складе"
    /// </summary>
    public Dictionary<string, string>? Warehouses { get; set; }
}