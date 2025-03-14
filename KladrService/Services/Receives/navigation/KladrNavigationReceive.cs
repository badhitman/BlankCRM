////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrNavigationReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrFindRequestModel?, Dictionary<KladrChainTypesEnum, JObject[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive;

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>?> ResponseHandleAction(KladrFindRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsListForParent(req);
    }
}