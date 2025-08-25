////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RubricsInstrumentUpdateModel
/// </summary>
public class RubricsInstrumentUpdateModel
{
    /// <inheritdoc/>
    public int InstrumentId { get; set; }

    /// <inheritdoc/>
    public int[]? RubricsIds { get; set; }
}