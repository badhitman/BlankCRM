////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ParameterElementDaichiJsonModel
/// </summary>
public class ParameterElementDaichiJsonModel : ParameterElementDaichiModel
{
    /// <inheritdoc/>
    public string[]? SECTIONS { get; set; }
    
    /// <inheritdoc/>
    public string[]? PHOTOES { get; set; }

    /// <inheritdoc/>
    public List<AttributeParameterDaichiModel>? Attributes { get; set; }
}