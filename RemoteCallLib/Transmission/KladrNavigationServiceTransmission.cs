////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// KladrNavigationServiceTransmission
/// </summary>
public class KladrNavigationServiceTransmission(IRabbitClient rabbitClient) : IKladrNavigationService
{
    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> ObjectsList(KladrsListRequestModel req)
        => await rabbitClient.MqRemoteCall<List<UniversalBaseModel>>(GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive) ?? [];
}
