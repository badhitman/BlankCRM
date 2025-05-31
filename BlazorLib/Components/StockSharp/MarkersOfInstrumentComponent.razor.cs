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

    private IEnumerable<MarkersInstrumentStockSharpEnum> _options
    {
        get => AllMarkers.Where(x => orignMarkers.Any(y => y.MarkerDescriptor == x));
        set => InvokeAsync(async () => await SetMarkers(value));
    }

    async Task SetMarkers(IEnumerable<MarkersInstrumentStockSharpEnum>? set)
    {
        await SetBusyAsync();
        SetMarkersForInstrumentRequestModel req = new()
        {
            InstrumentId = Instrument.Id,
            SetMarkers = set is null ? null : [.. set]
        };
        ResponseBaseModel? resUpd = await SsRepo.SetMarkersForInstrumentAsync(req);
        SnackbarRepo.ShowMessagesResponse(resUpd.Messages);
        TResponseModel<List<MarkerInstrumentStockSharpViewModel>> res = await SsRepo.GetMarkersForInstrumentAsync(Instrument.Id);
        orignMarkers = res.Response ?? [];
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
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