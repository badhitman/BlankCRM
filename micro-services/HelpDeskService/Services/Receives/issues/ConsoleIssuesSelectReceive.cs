////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// ConsoleIssuesSelectReceive
/// </summary>
public class ConsoleIssuesSelectReceive(IHelpDeskService hdRepo) : IResponseReceive<TPaginationRequestStandardModel<ConsoleIssuesRequestModel>?, TPaginationResponseStandardModel<IssueHelpDeskModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ConsoleIssuesSelectHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<IssueHelpDeskModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<ConsoleIssuesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.ConsoleIssuesSelectAsync(req, token);
    }
}