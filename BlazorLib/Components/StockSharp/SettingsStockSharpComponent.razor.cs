////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// SettingsStockSharpComponent
/// </summary>
public partial class SettingsStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StorageRepo { get; set; } = default!;

    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;


    readonly List<BoardStockSharpViewModel> Boards = [];

    IEnumerable<int>? _selectedBoards;
    IEnumerable<BoardStockSharpViewModel> SelectedBoards
    {
        get
        {
            if (_selectedBoards is null)
                return [];

            return Boards.Where(x => _selectedBoards.Any(y => x.Id == y));
        }
        set
        {
            _selectedBoards = value.Select(x => x.Id);
            InvokeAsync(SaveParameters);
        }
    }

    IEnumerable<MarkersInstrumentStockSharpEnum?>? _markersSelected = [];
    IEnumerable<MarkersInstrumentStockSharpEnum?> MarkersSelected
    {
        get => _markersSelected ?? [];
        set
        {
            _markersSelected = value;
            InvokeAsync(SaveParameters);
        }
    }

    async Task ReloadBoards()
    {
        TResponseModel<List<BoardStockSharpViewModel>> boardsRes = await SsRepo.GetBoardsAsync();

        lock (Boards)
        {
            Boards.Clear();
            if (boardsRes.Response is not null)
                Boards.AddRange(boardsRes.Response);
        }
    }

    async Task SaveParameters()
    {
        if (_markersSelected is not null)
            await StorageRepo.SaveParameterAsync(_markersSelected.ToArray(), GlobalStaticCloudStorageMetadata.MarkersDashboard, true);

        if (_selectedBoards is not null)
            await StorageRepo.SaveParameterAsync(_selectedBoards.ToArray(), GlobalStaticCloudStorageMetadata.BoardsDashboard, true);
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

        await Task.WhenAll([
            Task.Run(ReloadBoards),
            Task.Run(async () => {
                TResponseModel<int[]> _readBoardsFilter = await StorageRepo.ReadParameterAsync<int[]>(GlobalStaticCloudStorageMetadata.BoardsDashboard);
                _selectedBoards = _readBoardsFilter.Response;
            }),
            Task.Run(async () => {
                TResponseModel<MarkersInstrumentStockSharpEnum?[]> _readMarkersFilter = await StorageRepo.ReadParameterAsync<MarkersInstrumentStockSharpEnum?[]>(GlobalStaticCloudStorageMetadata.MarkersDashboard);
                _markersSelected = _readMarkersFilter.Response is null
                    ? []
                    : [.. _readMarkersFilter.Response];
            })]);

        await SetBusyAsync(false);
    }
}