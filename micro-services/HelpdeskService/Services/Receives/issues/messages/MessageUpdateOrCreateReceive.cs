////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Сообщение в обращение
/// </summary>
public class MessageUpdateOrCreateReceive(IHelpDeskService hdRepo) : IResponseReceive<TAuthRequestModel<IssueMessageHelpDeskBaseModel>?, TResponseModel<int?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueUpdateHelpDeskReceive;

    /// <summary>
    /// Сообщение в обращение
    /// </summary>
    public async Task<TResponseModel<int?>?> ResponseHandleActionAsync(TAuthRequestModel<IssueMessageHelpDeskBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.MessageUpdateOrCreateAsync(req, token);
    }
}