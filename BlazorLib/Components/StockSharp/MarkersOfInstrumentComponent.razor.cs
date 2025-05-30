////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// MarkersOfInstrumentComponent
/// </summary>
public partial class MarkersOfInstrumentComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Instrument
    /// </summary>
    [Parameter, EditorRequired]
    public required InstrumentTradeStockSharpViewModel Instrument {  get; set; }
}