////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Единицы измерения
/// </summary>
public class UnitsRusklimatResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public int TotalCount { get; set; }

    /// <inheritdoc/>
    public UnitRusklimatModelDB[]? Data { get; set; }
}