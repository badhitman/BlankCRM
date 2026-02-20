////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHistoryIndexing
/// </summary>
public interface IHistoryIndexing
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveHistoryForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryBaseAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForOrdersRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForDeliveriesRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForConversionsRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForPaymentsRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default);
}