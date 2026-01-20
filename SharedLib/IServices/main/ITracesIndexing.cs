////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ITracesIndexing
/// </summary>
public interface ITracesIndexing
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveTraceForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectForOrdersRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default);
}