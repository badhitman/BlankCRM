////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// BreezRuGoodsModel
/// </summary>
public class BreezRuGoodsModel : BreezRuGoodsBaseModel
{
    /// <summary>
    /// Цены
    /// </summary>
    [JsonProperty("price"), JsonPropertyName("price")]
    public BreezRuPriceModel? Price { get; set; }
}