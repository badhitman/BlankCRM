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

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required TradingAreaComponent Parent { get; set; }


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

    decimal _valueOperation;
    /// <inheritdoc/>
    public decimal ValueOperation
    {
        get => _valueOperation;
        private set
        {
            _valueOperation = value;
        }
    }

    decimal _shiftPosition;
    /// <inheritdoc/>
    public decimal ShiftPosition
    {
        get => _shiftPosition;
        private set
        {
            _shiftPosition = value;
        }
    }
}