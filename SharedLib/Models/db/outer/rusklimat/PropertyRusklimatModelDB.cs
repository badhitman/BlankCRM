////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Свойства товаров
/// </summary>
[Index(nameof(Sort))]
public class PropertyRusklimatModelDB : EntryAltStandardModel
{
    /// <inheritdoc/>
    public required int Sort { get; set; }
}