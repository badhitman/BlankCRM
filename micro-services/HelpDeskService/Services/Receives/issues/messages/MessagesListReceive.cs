////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Получить сообщения для инцидента
/// </summary>
public class MessagesListReceive(IHelpDeskService hdRepo)
    : IResponseReceive<TAuthRequestModel<int>?, TResponseModel<IssueMessageHelpDeskModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessagesOfIssueListHelpDeskReceive;

    /// <summary>
    /// Получить сообщения для инцидента
    /// </summary>
    public async Task<TResponseModel<IssueMessageHelpDeskModelDB[]>?> ResponseHandleActionAsync(TAuthRequestModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.MessagesListAsync(req, token);
    }
}