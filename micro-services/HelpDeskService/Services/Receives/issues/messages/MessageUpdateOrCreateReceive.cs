////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Сообщение в обращение
/// </summary>
public class MessageUpdateOrCreateReceive(IHelpDeskService hdRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel>?, TResponseModel<int?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueUpdateHelpDeskReceive;

    /// <summary>
    /// Сообщение в обращение
    /// </summary>
    public async Task<TResponseModel<int?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<int?> res = await hdRepo.MessageUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}