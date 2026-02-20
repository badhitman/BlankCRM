////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.TraceHistoryView;

/// <summary>
/// HistoryMainWrapperComponent
/// </summary>
public partial class HistoryMainWrapperComponent : HistoryOwnerBaseComponent
{
    /// <inheritdoc/>
    public override async Task<TableData<TraceReceiverRecord>> ServerReload(DateRange? dateRangePeriod, TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            Payload = new()
            {
                Start = dateRangePeriod?.Start,
                End = dateRangePeriod?.End,
            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await IndexingRepo.SelectHistoryBaseAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<TraceReceiverRecord>()
        {
            TotalItems = res.TotalRowsCount,
            Items = res.Response
        };
    }
}