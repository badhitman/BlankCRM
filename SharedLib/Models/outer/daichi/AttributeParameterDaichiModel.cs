////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// AttributeParameterDaichiModel
/// </summary>
[Index(nameof(CODE)), Index(nameof(NAME)), Index(nameof(VALUE)), Index(nameof(GROUP))]
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