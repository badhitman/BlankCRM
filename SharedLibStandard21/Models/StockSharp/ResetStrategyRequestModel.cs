////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ResetStrategyRequestModel
/// </summary>
public class ResetStrategyRequestModel
{
    /// <inheritdoc/>
    public int InstrumentId { get; set; }


    /// <inheritdoc/>
    public decimal Volume { get; set; }

    /// <inheritdoc/>
    public decimal Size { get; set; }
}