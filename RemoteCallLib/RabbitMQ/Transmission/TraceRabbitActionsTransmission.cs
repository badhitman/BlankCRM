////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// TraceRabbitActionsTransmission
/// </summary>
public class TraceRabbitActionsTransmission([FromKeyedServices(nameof(RabbitClient))] IMQStandardClientRPC rabbitClient) : ITraceRabbitActionsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.TraceRabbitActionSystemsReceive, req, waitResponse: false, token: token) ?? new();
}
