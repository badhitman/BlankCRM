////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// Технические характеристики Категории
/// </summary>
/// <remarks>
/// В ответ на данный запрос возвращаются все технические характеристики, принадлежащие к категории с указанным идентификатором.
/// В случае, если категории с указанным идентификатором не существует, в JSON возвращается объект с ключом "error" и значением "Неверный ID".
/// В XML возвращается объект "categories" с двумя объектами: "cat_id" содержащий запрошенный идентификатор и пустой "techs".
/// </remarks>
public class TechCategoryBreezRuModel : TechBreezRuBaseModel
{
    /// <inheritdoc/>
    [JsonProperty("tech_id"), JsonPropertyName("tech_id")]
    public required int TechId { get; set; }

    /// <inheritdoc/>
    [JsonProperty("data_type"), JsonPropertyName("data_type")]
    public string? DataType { get; set; }
}