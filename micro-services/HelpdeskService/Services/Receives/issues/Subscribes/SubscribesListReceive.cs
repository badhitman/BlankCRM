////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Subscribes list - of context user
/// </summary>
public class SubscribesListReceive(IHelpdeskService hdRepo)
    : IResponseReceive<TAuthRequestModel<int>?, TResponseModel<List<SubscriberIssueHelpdeskModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SubscribesIssueListHelpdeskReceive;

    /// <summary>
    /// Подписчики на события в обращении/инциденте
    /// </summary>
    public async Task<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>?> ResponseHandleActionAsync(TAuthRequestModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.SubscribesListAsync(req, token);
    }
}