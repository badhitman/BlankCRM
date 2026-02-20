////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class HistoryTransmission(IMQStandardClientRPC rabbitClient) : IHistoryIndexing
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveHistoryForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveTraceForReceiverReceive, req, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryBaseAsync(TPaginationRequestStandardModel<SelectHistoryReceivesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForConversionsRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForConversionsRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForDeliveriesRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForDeliveriesRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForOrdersRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForOrdersRetailReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> SelectHistoryForPaymentsRetailAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForPaymentsRetailReceive, req, token: token) ?? new();
}