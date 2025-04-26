////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Электронная торговая биржа
/// </summary>
[Index(nameof(LastAtUpdatedUTC)), Index(nameof(Name))]
public class ExchangeBoardModelDB : ExchangeBoardModel, IBaseStockSharpModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public new required string Name { get; set; }

    /// <summary>
    /// Инструменты (биржевые торговые)
    /// </summary>
    public List<InstrumentTradeModelDB>? Instruments { get; set; }

    /// <inheritdoc/>
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}