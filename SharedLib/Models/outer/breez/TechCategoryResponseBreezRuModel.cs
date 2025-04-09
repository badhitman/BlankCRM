////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TechCategoryResponseBreezRuModel
/// </summary>
public class TechCategoryResponseBreezRuModel
{
    /// <inheritdoc/>
    public int Category { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, TechCategoryBreezRuModel>? Techs { get; set; }
}