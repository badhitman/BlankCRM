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
    public async Task<ResponseBaseModel> ClearTempKladr()
        => await rabbitClient.MqRemoteCall<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.ClearTempKladrReceive) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> FlushTempKladr()
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.FlushTempKladrRecive) ?? new();

    /// <inheritdoc/>
    public async Task<MetadataKladrModel> GetMetadataKladr(GetMetadataKladrRequestModel req)
        => await rabbitClient.MqRemoteCall<MetadataKladrModel>(GlobalStaticConstants.TransmissionQueues.GetMetadataKladrReceive, req) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RegisterJobTempKladr(RegisterJobTempKladrRequestModel req)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.RegisterJobTempKladrReceive, req, waitResponse: false) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UploadPartTempKladr(UploadPartTableDataModel req)
        => await rabbitClient.MqRemoteCall<MetadataKladrModel>(GlobalStaticConstants.TransmissionQueues.UploadPartTempKladrReceive, req, waitResponse: false) ?? new();
}