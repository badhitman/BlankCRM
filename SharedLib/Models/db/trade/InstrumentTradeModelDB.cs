////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InstrumentTradeModelDB
/// </summary>
public class InstrumentTradeModelDB : EntryUpdatedModel
{
    /// <inheritdoc/>
    public required string IdRemote { get; set; }

    /// <inheritdoc/>
    public required virtual string Code {  get; set; }

    /// <inheritdoc/>
    public ExchangeBoardModelDB? ExchangeBoard { get; set; }
    /// <inheritdoc/>
    public int ExchangeBoardId { get; set; }
}
