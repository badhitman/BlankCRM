////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.realtime;

/// <summary>
/// GetClientsConnections
/// </summary>
public class GetClientsConnectionsReceive(IWebChatService webChatRepo)
    : IResponseReceive<GetClientsRequestModel?, TResponseModel<List<MqttClientModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetClientsConnectionsReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<MqttClientModel>>?> ResponseHandleActionAsync(GetClientsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TResponseModel<List<MqttClientModel>> res = await webChatRepo.GetClientsConnectionsAsync(req, token);
        return res;
    }
}