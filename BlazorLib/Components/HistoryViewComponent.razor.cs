////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// HistoryViewComponent
/// </summary>
public partial class HistoryViewComponent
{
    [Inject]
    ITracesIndexing IndexingRepo { get; set; } = default!;

    /// <summary>
    /// JS
    /// </summary>
    [Inject]
    IJSRuntime JS { get; set; } = default!;


    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        await JS.InvokeVoidAsync("HighlightBlock.Init");
    }

    async Task<TableData<TraceReceiverRecord>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            Payload = new()
            {

            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await IndexingRepo.TracesSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<TraceReceiverRecord>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}