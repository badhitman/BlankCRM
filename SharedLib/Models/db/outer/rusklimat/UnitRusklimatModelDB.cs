////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Единицы измерения
/// </summary>
public class UnitRusklimatModelDB : EntryAltStandardModel
{
    /// <inheritdoc/>
    public string? NameFull { get; set; }

    /// <inheritdoc/>
    public required string Code { get; set; }

    /// <summary>
    /// международное сокращение
    /// </summary>
    public string? IntAbbr { get; set; }
}