////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Прочитать рубрику (со всеми вышестоящими владельцами)
/// </summary>
public class RubricReadReceive(IRubricsService hdRepo)
    : IResponseReceive<int, TResponseModel<List<RubricStandardModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesReadHelpDeskReceive;

    /// <summary>
    /// Прочитать рубрику (со всеми вышестоящими владельцами)
    /// </summary>
    public async Task<TResponseModel<List<RubricStandardModel>>?> ResponseHandleActionAsync(int rubricId, CancellationToken token = default)
    {
        return await hdRepo.RubricReadWithParentsHierarchyAsync(rubricId, token);
    }
}