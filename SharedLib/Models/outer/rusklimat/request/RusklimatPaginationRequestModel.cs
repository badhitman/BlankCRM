////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// Запрос (с пагинацией)
/// </summary>
public class RusklimatPaginationRequestModel
{
    /// <inheritdoc/>
    [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
    public int PageNum { get; set; }

    /// <inheritdoc/>
    [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
    public int PageSize { get; set; }

    /// <inheritdoc/>
    [JsonProperty("sort"), JsonPropertyName("sort")]
    public Dictionary<string, string> Sort { get; set; } = new Dictionary<string, string>() { { "nsCode", "asc" } };
}