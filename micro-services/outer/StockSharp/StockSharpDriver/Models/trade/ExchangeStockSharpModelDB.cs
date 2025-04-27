////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
    public List<BoardStockSharpModelDB> Boards { get; set; }

    /// <inheritdoc/>
    public void SetUpdate(ExchangeStockSharpModel req)
    {
        CountryCode = req.CountryCode;
        Name = req.Name;
    }
}