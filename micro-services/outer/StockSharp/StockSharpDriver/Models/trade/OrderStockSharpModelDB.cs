////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

[Index(nameof(LastUpdatedAtUTC)), Index(nameof(Id)), Index(nameof(StringId)), Index(nameof(BoardId)), Index(nameof(TransactionId)), Index(nameof(BrokerCode))]
/// <inheritdoc/>
public class OrderStockSharpModelDB : OrderStockSharpModel, IBaseStockSharpModel
{
    [Key]
    public int IdPK { get; set; }

    ///<inheritdoc/>
    public new InstrumentStockSharpModelDB Instrument { get; set; }
    ///<inheritdoc/>
    public int InstrumentId { get; set; }

    ///<inheritdoc/>
    public new PortfolioTradeModelDB Portfolio { get; set; }
    ///<inheritdoc/>
    public int PortfolioId { get; set; }

    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }

    internal void SetUpdate(OrderStockSharpModel req)
    {
        throw new NotImplementedException();
    }
}