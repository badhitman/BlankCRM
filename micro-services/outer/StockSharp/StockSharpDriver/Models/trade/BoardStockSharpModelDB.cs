////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Площадка
/// </summary>
[Index(nameof(LastUpdatedAtUTC)), Index(nameof(Code))]
public class BoardStockSharpModelDB : BoardStockSharpModel, IBaseStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Exchange
    /// </summary>
    public new ExchangeStockSharpModelDB Exchange { get; set; }
    /// <summary>
    /// Exchange
    /// </summary>
    public int? ExchangeId { get; set; }

    /// <summary>
    /// Инструменты (биржевые торговые)
    /// </summary>
    public List<InstrumentStockSharpModelDB> Instruments { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public void SetUpdate(BoardStockSharpModel req)
    {
        Code = req.Code;
        LastUpdatedAtUTC = DateTime.UtcNow;
    }
}