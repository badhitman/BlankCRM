////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Read issue - of context user
/// </summary>
public class IssuesReadReceive(IHelpDeskService hdRepo) : IResponseReceive<TAuthRequestModel<IssuesReadRequestModel>?, TResponseModel<IssueHelpDeskModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IssuesGetHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueHelpDeskModelDB[]>?> ResponseHandleActionAsync(TAuthRequestModel<IssuesReadRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.IssuesReadAsync(req, token);
    }
}