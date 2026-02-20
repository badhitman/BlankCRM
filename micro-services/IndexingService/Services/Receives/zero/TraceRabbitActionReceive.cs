////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// TraceRabbitAction
/// </summary>
public class TraceRabbitActionReceive(ITraceRabbitActionsService webChatRepo)
    : IMQStandardReceive<TraceRabbitActionRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TraceRabbitActionSystemsReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TraceRabbitActionRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await webChatRepo.SaveActionAsync(req, token);
    }
}