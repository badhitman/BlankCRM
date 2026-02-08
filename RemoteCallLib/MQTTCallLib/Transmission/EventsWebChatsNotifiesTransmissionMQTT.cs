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
    public async Task<ResponseBaseModel> OnClientConnectedWebChatAsync(ConnectionOpenWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClientDisconnectedWebChatAsync(ConnectionCloseWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InitWebChatAsync(InitWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.InitWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingClientsWebChatAsync(PingClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingClientsWebChatNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PongClientWebChatAsync(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.PongClientWebChatNotifyReceive, req.ResponseContainerGUID), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StateGetWebChatAsync(GetStateWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateGetWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StateSetWebChatAsync(StateWebChatModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateSetWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> StateEchoWebChatAsync(StateWebChatModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateEchoWebChatNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ToastShowAsync(ShowToastEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ToastShowNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();
}