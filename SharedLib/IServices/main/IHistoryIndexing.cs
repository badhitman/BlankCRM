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
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryBaseAsync(TPaginationRequestStandardModel<SelectHistoryReceivesRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForOrdersRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForDeliveriesRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForConversionsRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForPaymentsRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default);
}