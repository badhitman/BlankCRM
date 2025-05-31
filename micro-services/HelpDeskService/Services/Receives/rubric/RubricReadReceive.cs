////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Прочитать рубрику (со всеми вышестоящими владельцами)
/// </summary>
public class RubricReadReceive(IHelpDeskService hdRepo) : IResponseReceive<int, TResponseModel<List<RubricIssueHelpDeskModelDB>?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesReadHelpDeskReceive;

    /// <summary>
    /// Прочитать рубрику (со всеми вышестоящими владельцами)
    /// </summary>
    public async Task<TResponseModel<List<RubricIssueHelpDeskModelDB>?>?> ResponseHandleActionAsync(int rubricId, CancellationToken token = default)
    {
        return await hdRepo.RubricReadAsync(rubricId, token);
    }
}