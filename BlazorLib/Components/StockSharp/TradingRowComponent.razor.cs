////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// TradingRowComponent
/// </summary>
public partial class TradingRowComponent : StockSharpBaseComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }


    decimal _basePrice;
    /// <inheritdoc/>
    public decimal BasePrice
    {
        get => _basePrice;
        private set
        {
            _basePrice = value;
        }
    }
}