////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IEventsWebChatsNotifies
/// </summary>
public interface IEventsWebChatsNotifies
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> NewMessageWebChatHandle(NewMessageWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PingClientsWebChatHandle(PingClientsWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> ClientDisconnectedWebChatHandle(ConnectCloseWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> OnClientConnectedWebChatHandle(ConnectOpenWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PongClientWebChatHandle(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> InitWebChatHandle(InitWebChatEventModel req, CancellationToken cancellationToken = default);
}