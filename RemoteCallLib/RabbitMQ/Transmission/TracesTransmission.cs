////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class TracesTransmission(IRabbitClient rabbitClient) : ITracesIndexing
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveTraceForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveTraceForReceiverReceive, req, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectForOrdersAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<TraceReceiverRecord>>(GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForOrdersRetailReceive, req, token: token) ?? new();
}