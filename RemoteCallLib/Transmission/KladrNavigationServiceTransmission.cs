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
    public async Task<Dictionary<KladrTypesResultsEnum, JObject[]>> ObjectsListForParent(KladrsListModel req)
        => await rabbitClient.MqRemoteCall<Dictionary<KladrTypesResultsEnum, JObject[]>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive, req) ?? [];
}