////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Категории товаров
/// </summary>
[Index(nameof(Parent))]
public class CategoryRusklimatModelDB : EntryAltModel
{
    /// <inheritdoc/>
    public string? Parent { get; set; }
}