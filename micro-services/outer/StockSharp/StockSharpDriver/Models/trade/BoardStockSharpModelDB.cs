////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Площадка
/// </summary>
[Index(nameof(LastAtUpdatedUTC)), Index(nameof(Code))]
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
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    /// <inheritdoc/>
    public void SetUpdate(BoardStockSharpModelDB req)
    {
        Exchange = req.Exchange;
        Instruments = req.Instruments;
        Code = req.Code;
        CreatedAtUTC = req.CreatedAtUTC;
        ExchangeId = req.ExchangeId;
        LastAtUpdatedUTC = DateTime.UtcNow;
    }
}