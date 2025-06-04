////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// RubricsOfInstrumentComponent
/// </summary>
public partial class RubricsOfInstrumentComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;


    /// <summary>
    /// Instrument
    /// </summary>
    [Parameter, EditorRequired]
    public required InstrumentTradeStockSharpViewModel Instrument { get; set; }

    
    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        //TResponseModel<List<MarkerInstrumentStockSharpViewModel>> res = await SsRepo.GetMarkersForInstrumentAsync(Instrument.Id);
        //originMarkers = res.Response ?? [];
        //_selectedOptions = originMarkers.Select(x=>x.MarkerDescriptor);
        //SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}