////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// StatusChangeReceive
/// </summary>
public class StatusChangeReceive(IHelpDeskService hdRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestModel<StatusChangeRequestModel>?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeIssueHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(TAuthRequestModel<StatusChangeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        TResponseModel<bool> res = await hdRepo.IssueStatusChangeAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}