////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InitialLoadRequestModel
/// </summary>
public class InitialLoadRequestModel
{
    /// <inheritdoc/>
    public decimal QuoteVolume { get; set; }

    /// <inheritdoc/>
    public decimal QuoteSizeVolume { get; set; }

    /// <inheritdoc/>
    public decimal SkipSizeVolume { get; set; }
}