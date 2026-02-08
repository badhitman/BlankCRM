////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;
using System.IO;

namespace RemoteCallLib;

/// <summary>
/// EventsWebChatsNotifiesTransmissionMQTT
/// </summary>
/// <param name="mqClient"></param>
public partial class EventsWebChatsNotifiesTransmissionMQTT(IMQTTClient mqClient) : IEventsWebChatsNotifies
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> NewMessageWebChatAsync(NewMessageWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClientDisconnectedWebChatAsync(ConnectCloseWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OnClientConnectedWebChatAsync(ConnectOpenWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InitWebChatAsync(InitWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InitWebChatNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingClientsWebChatAsync(PingClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingClientsWebChatNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PongClientWebChatAsync(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.PongClientWebChatNotifyReceive, req.ResponseContainerGUID), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> GetStateWebChatAsync(GetStateWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.GetStateWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StateWebChatSetAsync(StateWebChatModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.SetStateWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StateWebChatEchoAsync(StateWebChatModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateWebChatEchoNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ShowToastAsync(ShowToastEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ShowToastNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();
}