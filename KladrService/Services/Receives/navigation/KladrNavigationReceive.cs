////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrNavigationReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrsRequestBaseModel?, Dictionary<KladrChainTypesEnum, JObject[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive;

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>?> ResponseHandleAction(KladrsRequestBaseModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsListForParent(req);
    }
}