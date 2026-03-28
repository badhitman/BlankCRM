////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// FirebaseTransmissionRabbit
/// </summary>
public class FirebaseTransmissionRabbit([FromKeyedServices(nameof(RabbitClient))] IMQStandardClientRPC rabbitClient) : IFirebaseService, IFirebaseServiceTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<FirebaseSDKConfigModel>> GetFirebaseConfigAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FirebaseSDKConfigModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetFirebaseConfigReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<string>>> SendFirebaseMessageAsync(TAuthRequestStandardModel<SendFirebaseMessageRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<string>>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendFirebaseMessageReceive, req, token: token) ?? new();
}