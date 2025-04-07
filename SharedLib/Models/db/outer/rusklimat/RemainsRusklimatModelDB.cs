////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// остатки
/// </summary>
public class RemainsRusklimatModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Product
    /// </summary>
    public ProductRusklimatModelDB? Product { get; set; }
    /// <summary>
    /// Product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Остатки на складах
    /// </summary>
    public List<WarehouseRemainsRusklimatModelDB>? WarehousesRemains { get; set; }

    /// <summary>
    /// сумма остатков по всем складам
    /// </summary>
    public string? Total { get; set; }
}