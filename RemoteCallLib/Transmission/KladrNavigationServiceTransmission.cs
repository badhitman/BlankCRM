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
    public async Task<ResponseBaseModel> ChildsContainsAsync(string codeLike, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationChildsContainsReceive, codeLike, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<KladrResponseModel>> ObjectGetAsync(KladrsRequestBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<KladrResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationGetObjectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsFindAsync(KladrFindRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<KladrResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationFindReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>> ObjectsListForParentAsync(KladrsRequestBaseModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<Dictionary<KladrChainTypesEnum, JObject[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationListReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>> ObjectsSelectAsync(KladrSelectRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<KladrResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationSelectReceive, req, token: token) ?? new();
}