////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// LockTransactionModelDB
/// </summary>
[Index(nameof(LockerId), nameof(LockerName), nameof(LockerAreaId), IsUnique = true)]
public class LockTransactionModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// LockerName
    /// </summary>
    public required string LockerName { get; set; }

    /// <summary>
    /// LockerId
    /// </summary>
    public required int LockerId { get; set; }

    /// <inheritdoc/>
    public required int LockerAreaId { get; set; }

    /// <summary>
    /// Метка для определения инициатора блокировки
    /// </summary>
    /// <remarks>
    /// Для тех случаев если блокировка не удалится автоматически. Маркер поможет понять где была создана эта блокировка.
    /// </remarks>
    public required string Marker { get; set; }
}