////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// IssuesSelectReceive
/// </summary>
public class IssuesSelectReceive(IHelpdeskService hdRepo)
    : IResponseReceive<TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>>?, TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.IssuesSelectHelpdeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>?> ResponseHandleActionAsync(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.IssuesSelect(req, token);
    }
}