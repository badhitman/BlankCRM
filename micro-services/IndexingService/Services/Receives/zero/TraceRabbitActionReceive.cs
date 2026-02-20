////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// TraceRabbitAction
/// </summary>
public class TraceRabbitActionReceive(IWebChatService webChatRepo)
    : IMQStandardReceive<MessageWebChatModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TraceRabbitActionSystemsReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(MessageWebChatModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.CreateMessageWebChatAsync(req, token);
    }
}