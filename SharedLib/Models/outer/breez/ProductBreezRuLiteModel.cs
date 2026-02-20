////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// ProductBreezRuLiteModel
/// </summary>
[Index(nameof(NC)), Index(nameof(VnutrNC)), Index(nameof(NarujNC)), Index(nameof(AccessoryNC))]
public class ProductBreezRuLiteModel
{
    /// <summary>
    /// НС-код продукта (или комплекта в сложносоставных)
    /// </summary>
    public string? NC { get; set; }

    /// <summary>
    /// НС-коды составных частей комплекта в сложносоставных
    /// </summary>
    [JsonProperty("nc_vnutr"), JsonPropertyName("nc_vnutr")]
    public string? VnutrNC { get; set; }

    /// <summary>
    /// НС-коды составных частей комплекта в сложносоставных
    /// </summary>
    [JsonProperty("nc_naruj"), JsonPropertyName("nc_naruj")]
    public string? NarujNC { get; set; }

    /// <summary>
    /// НС-коды составных частей комплекта в сложносоставных
    /// </summary>
    [JsonProperty("nc_accessory"), JsonPropertyName("nc_accessory")]
    public string? AccessoryNC { get; set; }
}
