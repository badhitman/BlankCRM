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
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatHandleNotifyReceive, req.DialogId.ToString()), req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConnectionCloseWebChatHandle(ConnectCloseWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatHandleNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ConnectionOpenWebChatHandle(ConnectOpenWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatHandleNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> InitWebChatHandle(InitWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.InitWebChatHandleNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PingClientsWebChatHandle(PingClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PingClientsWebChatHandleNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> PongClientWebChatHandle(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default)
        => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.PongClientWebChatHandleNotifyReceive, req, waitResponse: false, token: cancellationToken) ?? new();
}