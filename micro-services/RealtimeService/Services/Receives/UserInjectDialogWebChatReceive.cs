////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// UserInjectDialogWebChat
/// </summary>
public class UserInjectDialogWebChatReceive(IWebChatService webChatRepo)
    : IResponseReceive<TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UserInjectDialogWebChatReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.UserInjectDialogWebChatAsync(req, token);
    }
}