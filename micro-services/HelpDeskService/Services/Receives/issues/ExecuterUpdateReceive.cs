////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Subscribe update - of context user
/// </summary>
public class ExecuterUpdateReceive(IHelpDeskService hdRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestModel<UserIssueModel>?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ExecuterIssueUpdateHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(TAuthRequestModel<UserIssueModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        TResponseModel<bool> res = await hdRepo.ExecuterUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}