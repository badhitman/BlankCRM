////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Биржа
/// </summary>
[Index(nameof(Name))]
public class ExchangeStockSharpModelDB : ExchangeStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Boards
    /// </summary>
    public List<BoardStockSharpModelDB>? Boards { get; set; }
}