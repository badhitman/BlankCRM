////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components;

/// <summary>
/// HistoryOwnerBaseComponent
/// </summary>
public abstract class HistoryOwnerBaseComponent : BlazorBusyComponentUsersCachedModel
{
    /// <inheritdoc/>
    [Inject]
    protected ITracesIndexing IndexingRepo { get; set; } = default!;


    /// <inheritdoc/>
    public abstract Task<TableData<TraceReceiverRecord>> ServerReload(DateRange? dateRangePeriod, TableState state, CancellationToken token);
}