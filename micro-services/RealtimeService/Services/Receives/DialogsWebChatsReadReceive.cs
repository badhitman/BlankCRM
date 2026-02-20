////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// DialogsWebChatsRead
/// </summary>
public class DialogsWebChatsReadReceive(IWebChatService webChatRepo)
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<List<DialogWebChatModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DialogsWebChatsReadReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<DialogWebChatModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.DialogsWebChatsReadAsync(req, token);
    }
}