////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InstrumentRubricUpdateModel
/// </summary>
public class InstrumentRubricUpdateModel
{
    /// <inheritdoc/>
    public bool Set { get; set; }
    
    /// <inheritdoc/>
    public int InstrumentId { get; set; }
    
    /// <inheritdoc/>
    public int RubricId { get; set; }
}
