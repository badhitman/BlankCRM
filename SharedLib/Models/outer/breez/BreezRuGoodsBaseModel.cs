////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// BreezRuGoodsBaseModel
/// </summary>
[Index(nameof(CodeNC)), Index(nameof(Articul)), Index(nameof(Quantity)), Index(nameof(Stock)), Index(nameof(TimeLastUpdate))]
public class BreezRuGoodsBaseModel
{
    /// <summary>
    /// НС-код
    /// </summary>
    [JsonProperty("nc"), JsonPropertyName("nc")]
    public string? CodeNC { get; set; }

    /// <summary>
    /// Артикул
    /// </summary>
    [JsonProperty("articul"), JsonPropertyName("articul")]
    public string? Articul { get; set; }

    /// <summary>
    /// Наименование товара
    /// </summary>
    [JsonProperty("title"), JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Остаток данного товара на данном складе (больше 50 единиц обозначаются как ">50")
    /// </summary>
    [JsonProperty("quantity"), JsonPropertyName("quantity")]
    public string? Quantity { get; set; }

    /// <summary>
    /// Наименование склада
    /// </summary>
    [JsonProperty("stock"), JsonPropertyName("stock")]
    public string? Stock { get; set; }

    /// <summary>
    /// Время последнего обновления данных
    /// </summary>
    [JsonProperty("time"), JsonPropertyName("time")]
    public string? TimeLastUpdate { get; set; }
}