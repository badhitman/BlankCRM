////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// TechBreezRuBaseModel
/// </summary>
public class TechBreezRuBaseModel
{
    /// <inheritdoc/>
    public string? Group { get; set; }

    /// <inheritdoc/>
    public string? Order { get; set; }
    /// <inheritdoc/>
    public string? Filter { get; set; }

    /// <inheritdoc/>
    [JsonProperty("filter_type"), JsonPropertyName("filter_type")]
    public string? FilterType { get; set; }

    /// <inheritdoc/>
    public string? Required { get; set; }

    /// <inheritdoc/>
    public required string Title { get; set; }

    /// <inheritdoc/>
    public string? Unit { get; set; }
}