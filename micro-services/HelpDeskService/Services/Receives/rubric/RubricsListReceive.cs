////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Получить рубрики, вложенные в рубрику (если не указано, то root перечень)
/// </summary>
public class RubricsListReceive(IRubricsService hdRepo)
    : IResponseReceive<RubricsListRequestModel?, List<UniversalBaseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesListHelpDeskReceive;

    /// <summary>
    /// Получить рубрики, вложенные в рубрику <paramref name="req"/>.OwnerId (если не указано, то root перечень)
    /// </summary>
    public async Task<List<UniversalBaseModel>?> ResponseHandleActionAsync(RubricsListRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.RubricsListAsync(req, token);
    }
}