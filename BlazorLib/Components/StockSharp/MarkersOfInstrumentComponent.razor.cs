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
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;


    /// <summary>
    /// Instrument
    /// </summary>
    [Parameter, EditorRequired]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }


    List<MarkerInstrumentStockSharpViewModel> orignMarkers = [];


    readonly MarkersInstrumentStockSharpEnum[] AllMarkers = Enum.GetValues<MarkersInstrumentStockSharpEnum>();

    //Enum.GetValues<MarkersInstrumentStockSharpEnum>()
    //MarkersInstrumentStockSharpEnum[] _states = []; // orignMarkers
    private IEnumerable<MarkersInstrumentStockSharpEnum> _options
    {
        get => AllMarkers.Where(x => orignMarkers.Any(y => y.MarkerDescriptor == x));
        set
        {
            //_states = (MarkersInstrumentStockSharpEnum[])value;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<List<MarkerInstrumentStockSharpViewModel>> res = await SsRepo.GetMarkersForInstrumentAsync(Instrument.Id);
        orignMarkers = res.Response ?? [];
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}