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
    public async Task<TResponseModel<KladrResponseModel>> ObjectGet(KladrsRequestBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<KladrResponseModel>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationGetObjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsFind(KladrFindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<KladrResponseModel>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationFindReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParent(KladrsRequestBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<Dictionary<KladrChainTypesEnum, JObject[]>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelect(KladrSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<KladrResponseModel>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationSelectReceive, req, token: token) ?? new();
}