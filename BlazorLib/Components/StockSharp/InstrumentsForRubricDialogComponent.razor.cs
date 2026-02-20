////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsForRubricDialogComponent
/// </summary>
public partial class InstrumentsForRubricDialogComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;


    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action OnRowClickHandle { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int RubricId { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required List<InstrumentTradeStockSharpViewModel> Instruments { get; set; }


    MudTable<InstrumentTradeStockSharpViewModel>? _table;
    void OnChipClosed(MudChip<InstrumentTradeStockSharpViewModel> chip)
        => InvokeAsync(async () => await InstrumentToggle(chip.Value!));
    async Task InstrumentToggle(InstrumentTradeStockSharpViewModel ctx)
    {
        bool _check = Instruments.Any(x => x.Id == ctx.Id);
        await SetBusyAsync(true);
        ResponseBaseModel res = await SsRepo.InstrumentRubricUpdateAsync(new() { InstrumentId = ctx.Id, RubricId = RubricId, Set = !_check });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (res.Success())
            lock (Instruments)
            {
                if (_check)
                    Instruments.RemoveAll(x => x.Id == ctx.Id);
                else
                    Instruments.Add(ctx);
            }

        if (_table is not null)
            await _table.ReloadServerData();

        await SetBusyAsync(false);
        OnRowClickHandle();
    }

    async Task<TableData<InstrumentTradeStockSharpViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        InstrumentsRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<InstrumentTradeStockSharpViewModel> res = await SsRepo.InstrumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpViewModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    void Cancel() => MudDialog.Cancel();
}