////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// ArticlesReadReceive
/// </summary>
public class ArticlesReadReceive(IArticlesService artRepo, ILogger<ArticlesReadReceive> loggerRepo) : IResponseReceive<int[]?, TResponseModel<ArticleModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ArticlesReadReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ArticleModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await artRepo.ArticlesReadAsync(req, token);
    }
}