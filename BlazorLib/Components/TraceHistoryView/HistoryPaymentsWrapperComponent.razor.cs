////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.TraceHistoryView;

/// <summary>
/// HistoryPaymentsWrapperComponent
/// </summary>
public partial class HistoryPaymentsWrapperComponent : HistoryOwnerBaseComponent
{
    /// <inheritdoc/>
    [Parameter]
    public int PaymentId { get; set; }

    /// <inheritdoc/>
    public override async Task<TableData<TraceReceiverRecord>> ServerReload(DateRange? dateRangePeriod, TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            Payload = new()
            {
                Start = dateRangePeriod?.Start,
                End = dateRangePeriod?.End,
                FilterId = PaymentId,
            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await IndexingRepo.SelectHistoryForPaymentsRetailAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<TraceReceiverRecord>()
        {
            TotalItems = res.TotalRowsCount,
            Items = res.Response
        };
    }
}