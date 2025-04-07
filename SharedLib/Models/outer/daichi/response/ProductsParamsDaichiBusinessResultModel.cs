////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// ProductsParamsDaichiBusinessResultModel
/// </summary>
public class ProductsParamsDaichiBusinessResultModel
{
    /// <summary>
    /// TotalCount
    /// </summary>
    [JsonProperty("total_count"), JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    [JsonProperty("data"), JsonPropertyName("data")]
    public Dictionary<string, JObject>? Data { get; set; }
}