////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// KladrNavigationServiceTransmission
/// </summary>
public class KladrNavigationServiceTransmission(IRabbitClient rabbitClient) : IKladrNavigationService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<KladrResponseModel>> ObjectGet(KladrsRequestBaseModel req)
        => await rabbitClient.MqRemoteCall<TResponseModel<KladrResponseModel>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationGetObjectReceive, req) ?? new();

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParent(KladrsRequestBaseModel req)
        => await rabbitClient.MqRemoteCall<Dictionary<KladrChainTypesEnum, JObject[]>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive, req) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelect(KladrSelectRequestModel req)
        => await rabbitClient.MqRemoteCall<TPaginationResponseModel<KladrResponseModel>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationSelectReceive, req) ?? new();
}