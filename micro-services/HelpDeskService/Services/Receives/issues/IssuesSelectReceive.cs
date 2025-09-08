////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// IssuesSelectReceive
/// </summary>
public class IssuesSelectReceive(IHelpDeskService hdRepo)
    : IResponseReceive<TAuthRequestModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>>?, TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IssuesSelectHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>?> ResponseHandleActionAsync(TAuthRequestModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.IssuesSelectAsync(req, token);
    }
}