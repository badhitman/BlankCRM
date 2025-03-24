﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Создать/обновить статью
/// </summary>
public class ArticleCreateOrUpdateReceive(IArticlesService artRepo, ILogger<ArticleCreateOrUpdateReceive> loggerRepo) : IResponseReceive<ArticleModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ArticleUpdateHelpdeskReceive;

    /// <summary>
    /// Создать/обновить статью
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleAction(ArticleModelDB? article, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(article);
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(article)}");
        return await artRepo.ArticleCreateOrUpdate(article, token);
    }
}