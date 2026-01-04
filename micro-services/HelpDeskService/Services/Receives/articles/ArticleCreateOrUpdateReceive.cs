////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Создать/обновить статью
/// </summary>
public class ArticleCreateOrUpdateReceive(IArticlesService artRepo, ILogger<ArticleCreateOrUpdateReceive> loggerRepo, IFilesIndexing indexingRepo)
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

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await artRepo.ArticleCreateOrUpdateAsync(req, token);
    }
}