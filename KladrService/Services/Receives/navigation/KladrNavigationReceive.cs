////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrNavigationReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrFindRequestModel?, Dictionary<KladrTypesResultsEnum, JObject[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive;

    /// <inheritdoc/>
    public async Task<Dictionary<KladrTypesResultsEnum, JObject[]>?> ResponseHandleAction(KladrFindRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsListForParent(req);
    }
}