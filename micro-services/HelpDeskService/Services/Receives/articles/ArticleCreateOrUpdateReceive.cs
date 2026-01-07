////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Создать/обновить статью
/// </summary>
public class ArticleCreateOrUpdateReceive(IArticlesService artRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ArticleModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ArticleUpdateHelpDeskReceive;

    /// <summary>
    /// Создать/обновить статью
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(ArticleModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id.ToString());
        TResponseModel<int> res = await artRepo.ArticleCreateOrUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}