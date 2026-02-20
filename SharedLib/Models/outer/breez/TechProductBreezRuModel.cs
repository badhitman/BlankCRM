////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// TechProductBreezRuModel
/// </summary>
[Index(nameof(IdChar)), Index(nameof(TypeParameter)), Index(nameof(Show)), Index(nameof(First)), Index(nameof(SubCategory)), Index(nameof(Analog)), Index(nameof(Value))]
public class PropTechProductBreezRuModel : TechBreezRuBaseModel
{
    /// <inheritdoc/>
    [JsonProperty("id_char"), JsonPropertyName("id_char")]
    public int IdChar { get; set; }

    /// <inheritdoc/>
    [JsonProperty("type"), JsonPropertyName("type")]
    public required string TypeParameter { get; set; }

    /// <summary>
    /// Отображается ли данная ТХ
    /// </summary>
    public required string Show { get; set; }

    /// <summary>
    /// Отображается ли ТХ в карточке товара
    /// </summary>
    public required string First { get; set; }

    /// <inheritdoc/>
    [JsonProperty("supcat"), JsonPropertyName("supcat")]
    public required string SubCategory { get; set; }

    /// <summary>
    /// Требуется ли ТХ для поиска аналогов
    /// </summary>
    public required string Analog { get; set; }

    /// <summary>
    /// Значение ТХ
    /// </summary>
    public required string Value { get; set; }
}