////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrNavigationListReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrsRequestBaseModel?, Dictionary<KladrChainTypesEnum, JObject[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive;

    /// <inheritdoc/>
    public async Task<Dictionary<KladrChainTypesEnum, JObject[]>?> ResponseHandleActionAsync(KladrsRequestBaseModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsListForParent(req, token);
    }
}