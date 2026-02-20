////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// TraceRabbitActionsTransmission
/// </summary>
public class TraceRabbitActionsTransmission([FromKeyedServices(nameof(NetMQClient))] IMQStandardClientRPC zeroClient) : ITraceRabbitActionsServiceTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default)
       => await zeroClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TraceRabbitActionSystemsReceive, req, waitResponse: false, token: token) ?? new();
}