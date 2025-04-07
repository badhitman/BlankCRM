////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AttributeParameterDaichiModel
/// </summary>
public class AttributeParameterDaichiModel
{
    /// <inheritdoc/>
    public required string CODE { get; set; }

    /// <inheritdoc/>
    public required string NAME { get; set; }

    /// <inheritdoc/>
    public required string VALUE { get; set; }

    /// <inheritdoc/>
    public string? GROUP { get; set; }
}