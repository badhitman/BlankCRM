////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrNavigationReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrsListRequestModel?, Dictionary<KladrTypesResultsEnum, JObject[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationListReceive;

    /// <inheritdoc/>
    public async Task<Dictionary<KladrTypesResultsEnum, JObject[]>?> ResponseHandleAction(KladrsListRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsList(req);
    }
}