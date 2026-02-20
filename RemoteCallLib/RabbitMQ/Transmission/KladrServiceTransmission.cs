////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// KladrServiceTransmission
/// </summary>
public class KladrServiceTransmission(IMQClientRPC rabbitClient) : IKladrService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClearTempKladrAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.ClearTempKladrReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FlushTempKladrAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.FlushTempKladrReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<MetadataKladrModel> GetMetadataKladrAsync(GetMetadataKladrRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<MetadataKladrModel>(GlobalStaticConstantsTransmission.TransmissionQueues.GetMetadataKladrReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadPartTempKladrAsync(UploadPartTableDataModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<MetadataKladrModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UploadPartTempKladrReceive, req, waitResponse: false, token: token) ?? new();
}