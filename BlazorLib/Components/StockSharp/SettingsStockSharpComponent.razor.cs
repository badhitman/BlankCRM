////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

public partial class SettingsStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;


    IEnumerable<MarkersInstrumentStockSharpEnum?> _markersSelected = [];
    IEnumerable<MarkersInstrumentStockSharpEnum?> MarkersSelected
    {
        get => _markersSelected;
        set
        {
            _markersSelected = value;
            InvokeAsync(SaveParameters);
        }
    }

    async Task SaveParameters()
    {
        if (MarkersSelected is not null)
            await StorageRepo.SaveParameterAsync(MarkersSelected, GlobalStaticCloudStorageMetadata.MarkersDashboard, true);
    }

    static string GetMultiSelectionText(List<string?> selectedValues)
    {
        return $"{string.Join(", ", selectedValues.Select(x => x is null ? "~not set~" : x))}";
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        TResponseModel<MarkersInstrumentStockSharpEnum?[]> _readMarkersFilter = await StorageRepo.ReadParameterAsync<MarkersInstrumentStockSharpEnum?[]>(GlobalStaticCloudStorageMetadata.MarkersDashboard);
        _markersSelected = _readMarkersFilter.Response is null
            ? [] 
            : [.. _readMarkersFilter.Response];
        await SetBusyAsync(false);
    }
}