////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.Json.Serialization;

namespace SharedLib;

/// <summary>
/// AgentModel
/// </summary>
public partial class AgentModel : IEquatable<AgentModel>
{
    /// <inheritdoc/>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("inn")]
    public string? Inn { get; set; }

    /// <inheritdoc/>
    [JsonPropertyName("kpp")]
    public string? Kpp { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"'{Name}' {Inn}/{Kpp}";
    }

    /// <inheritdoc/>
    public bool Equals(AgentModel? other)
    {
        return other is not null && other.Name == Name && other.Inn == Inn && other.Kpp == Kpp;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as AgentModel);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Inn, Kpp);
    }
}