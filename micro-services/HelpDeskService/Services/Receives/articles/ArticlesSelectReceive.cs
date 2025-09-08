﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// ArticlesSelectReceive
/// </summary>
public class ArticlesSelectReceive(IArticlesService artRepo, ILogger<ArticlesSelectReceive> loggerRepo) : IResponseReceive<TPaginationRequestStandardModel<SelectArticlesRequestModel>?, TPaginationResponseModel<ArticleModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesSelectHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ArticleModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectArticlesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await artRepo.ArticlesSelectAsync(req, token);
    }
}