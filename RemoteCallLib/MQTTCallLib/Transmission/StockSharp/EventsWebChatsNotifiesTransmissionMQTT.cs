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
    public async Task<ResponseBaseModel> NewMessageWebChatHandle(NewMessageWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatHandleNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ClientDisconnectedWebChatHandle(ConnectCloseWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatHandleNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> OnClientConnectedWebChatHandle(ConnectOpenWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatHandleNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InitWebChatHandle(InitWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InitWebChatHandleNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingClientsWebChatHandle(PingClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingClientsWebChatHandleNotifyReceive, req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PongClientWebChatHandle(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.PongClientWebChatHandleNotifyReceive, req.ResponseContainerGUID), req, waitResponse: false, propertyValue: new(GlobalStaticConstantsRoutes.Routes.MUTE_CONTROLLER_NAME, [1]), token: cancellationToken) ?? new();
}