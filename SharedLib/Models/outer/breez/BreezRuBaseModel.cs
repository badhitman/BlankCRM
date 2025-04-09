////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// BreezRuBaseModel
/// </summary>
public class BreezRuBaseModel
{
    /// <inheritdoc/>
    public required string Title { get; set; }

    /// <inheritdoc/>
    public string? CHPU { get; set; }

    /// <inheritdoc/>
    public string? Order { get; set; }
}