////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// CreateIssueTheme
/// </summary>
public class RubricCreateOrUpdateReceive(IRubricsService hdRepo)
    : IResponseReceive<RubricStandardModel?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(RubricStandardModel? rubric, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rubric);
        return await hdRepo.RubricCreateOrUpdateAsync(rubric, token);
    }
}