////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// MoveMetaObjectModel
/// </summary>
public class MoveMetaObjectModel : MoveObjectModel
{
    /// <inheritdoc/>
    public required string ApplicationName { get; set; }

    /// <inheritdoc/>
    public required string? PropertyName { get; set; }

    /// <inheritdoc/>
    public required string? PrefixPropertyName { get; set; }
}