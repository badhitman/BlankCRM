////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// ArticlesSelectReceive
/// </summary>
public class ArticlesSelectReceive(IArticlesService artRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectArticlesRequestModel>?, TPaginationResponseStandardModel<ArticleModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesSelectHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ArticleModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectArticlesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await artRepo.ArticlesSelectAsync(req, token);
    }
}