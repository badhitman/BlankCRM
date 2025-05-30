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
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }


    MarkersInstrumentStockSharpEnum[] _states = [];
    private IEnumerable<MarkersInstrumentStockSharpEnum> _options
    {
        get => _states;
        set
        {
            _states = (MarkersInstrumentStockSharpEnum[])value;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();

        await SetBusyAsync(false);
    }
}