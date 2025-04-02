////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// BreezRuElementModelDB
/// </summary>
[Index(nameof(CurrencyBasePrice)), Index(nameof(CurrencyRIC)), Index(nameof(LoadedDateTime))]
public class BreezRuElementModelDB : BreezRuGoodsBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Базовая цена
    /// </summary>
    public string? BasePrice { get; set; }

    /// <summary>
    /// Валюта базовой цены
    /// </summary>
    public string? CurrencyBasePrice { get; set; }

    /// <summary>
    /// РИЦ
    /// </summary>
    public string? RIC { get; set; }

    /// <summary>
    /// Валюта РИЦ
    /// </summary>
    public string? CurrencyRIC { get; set; }

    /// <summary>
    /// LoadedDateTime
    /// </summary>
    public DateTime LoadedDateTime { get; set; }

    /// <inheritdoc/>
    public static BreezRuElementModelDB Build(BreezRuGoodsModel x)
    {
        return new()
        {
            Articul = x.Articul,
            CodeNC = x.CodeNC,
            Quantity = x.Quantity,
            LoadedDateTime = DateTime.UtcNow,
            Stock = x.Stock,
            Title = x.Title,
            BasePrice = x.Price?.BasePrice,
            CurrencyBasePrice = x.Price?.CurrencyBasePrice,
            RIC = x.Price?.RIC,
            CurrencyRIC = x.Price?.CurrencyRIC,
            TimeLastUpdate = x.TimeLastUpdate,
        };
    }
}