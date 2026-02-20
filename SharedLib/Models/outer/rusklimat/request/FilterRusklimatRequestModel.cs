////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// FilterRusklimatRequestModel
/// </summary>
public class FilterRusklimatRequestModel
{
    /// <summary>
    /// массив id категорий, товары которых нужно получить, в ответе также будут товары, которые находятся в подкатегориях
    /// </summary>
    [JsonProperty("categoryIds"), JsonPropertyName("categoryIds")]
    public string[]? CategoryIds { get; set; }

    /// <inheritdoc/>
    [JsonProperty("extraStuff"), JsonPropertyName("extraStuff")]
    public Dictionary<string, string[]>? ExtraStuff { get; set; }
}