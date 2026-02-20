////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// BreezRuPriceModel
/// </summary>
public class BreezRuPriceModel
{
    /// <summary>
    /// Базовая цена
    /// </summary>
    [JsonProperty("base"), JsonPropertyName("base")]
    public string? BasePrice { get; set; }

    /// <summary>
    /// Валюта базовой цены
    /// </summary>
    [JsonProperty("base_currency"), JsonPropertyName("base_currency")]
    public string? CurrencyBasePrice { get; set; }

    /// <summary>
    /// РИЦ
    /// </summary>
    [JsonProperty("ric"), JsonPropertyName("ric")]
    public string? RIC { get; set; }

    /// <summary>
    /// Валюта РИЦ
    /// </summary>
    [JsonProperty("ric_currency"), JsonPropertyName("ric_currency")]
    public string? CurrencyRIC { get; set; }
}