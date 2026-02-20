////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// остатки
/// </summary>
[Index(nameof(Total))]
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
    /// Остатки на складах
    /// </summary>
    public List<WarehouseRemainsRusklimatModelDB>? WarehousesRemains { get; set; }

    /// <summary>
    /// сумма остатков по всем складам
    /// </summary>
    public string? Total { get; set; }
}