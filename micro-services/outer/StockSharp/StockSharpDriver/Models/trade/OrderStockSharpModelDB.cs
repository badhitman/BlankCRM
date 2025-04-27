////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

[Index(nameof(LastAtUpdatedUTC)), Index(nameof(Id)), Index(nameof(StringId)), Index(nameof(BoardId)), Index(nameof(TransactionId)), Index(nameof(BrokerCode))]
/// <inheritdoc/>
public class OrderStockSharpModelDB : OrderStockSharpModel, IBaseStockSharpModel
{
    ///<inheritdoc/>
    public new InstrumentTradeModelDB Instrument { get; set; }
    ///<inheritdoc/>
    public int InstrumentId { get; set; }

    ///<inheritdoc/>
    public new PortfolioTradeModelDB Portfolio { get; set; }
    ///<inheritdoc/>
    public int PortfolioId { get; set; }

    /// <inheritdoc/>
    public DateTime LastAtUpdatedUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }
}
