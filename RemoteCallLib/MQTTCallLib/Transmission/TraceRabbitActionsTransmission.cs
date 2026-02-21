////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCallLib;

/// <summary>
/// TraceRabbitActionsTransmission
/// </summary>
public class TraceRabbitActionsTransmission(IMQStandardClientExtRPC mqttClient) : ITraceRabbitActionsServiceTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default)
       => await mqttClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TraceRabbitActionSystemsReceive, req, waitResponse: false, token: token) ?? new();
}