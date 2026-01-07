////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Получить рубрики
/// </summary>
public class RubricsGetReceive(IRubricsService hdRepo)
    : IResponseReceive<int[]?, TResponseModel<List<RubricStandardModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesGetHelpDeskReceive;

    /// <summary>
    /// Получить рубрики
    /// </summary>
    public async Task<TResponseModel<List<RubricStandardModel>>?> ResponseHandleActionAsync(int[]? rubricsIds, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rubricsIds);
        return await hdRepo.RubricsGetAsync(rubricsIds, token);
    }
}