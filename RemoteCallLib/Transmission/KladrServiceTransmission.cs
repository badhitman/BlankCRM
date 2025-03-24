////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// KladrServiceTransmission
/// </summary>
public class KladrServiceTransmission(IRabbitClient rabbitClient) : IKladrService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearTempKladr(CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.ClearTempKladrReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FlushTempKladr(CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FlushTempKladrRecive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<MetadataKladrModel> GetMetadataKladr(GetMetadataKladrRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<MetadataKladrModel>(GlobalStaticConstants.TransmissionQueues.GetMetadataKladrReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadPartTempKladr(UploadPartTableDataModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<MetadataKladrModel>(GlobalStaticConstants.TransmissionQueues.UploadPartTempKladrReceive, req, waitResponse: false, token: token) ?? new();
}