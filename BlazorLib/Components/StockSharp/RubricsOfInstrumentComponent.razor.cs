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


    List<MarkerInstrumentStockSharpViewModel> originMarkers = [];

    readonly MarkersInstrumentStockSharpEnum[] AllMarkers = Enum.GetValues<MarkersInstrumentStockSharpEnum>();

    IEnumerable<MarkersInstrumentStockSharpEnum>? _selectedOptions;
    private IEnumerable<MarkersInstrumentStockSharpEnum>? SelectedOptions
    {
        get => _selectedOptions;
        set
        {
            _selectedOptions = value;
            InvokeAsync(SetMarkers);
        }
    }

    async Task SetMarkers()
    {
        await SetBusyAsync();
        SetMarkersForInstrumentRequestModel req = new()
        {
            InstrumentId = Instrument.Id,
            SetMarkers = SelectedOptions is null ? null : [.. SelectedOptions]
        };
        ResponseBaseModel? resUpd = await SsRepo.SetMarkersForInstrumentAsync(req);
        SnackbarRepo.ShowMessagesResponse(resUpd.Messages);
        TResponseModel<List<MarkerInstrumentStockSharpViewModel>> res = await SsRepo.GetMarkersForInstrumentAsync(Instrument.Id);
        originMarkers = res.Response ?? [];
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<List<MarkerInstrumentStockSharpViewModel>> res = await SsRepo.GetMarkersForInstrumentAsync(Instrument.Id);
        originMarkers = res.Response ?? [];
        _selectedOptions = originMarkers.Select(x=>x.MarkerDescriptor);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}