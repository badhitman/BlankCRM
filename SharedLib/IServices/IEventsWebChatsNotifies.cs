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
    public Task<ResponseBaseModel> NewMessageWebChatAsync(NewMessageWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PingClientsWebChatAsync(PingClientsWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> ClientDisconnectedWebChatAsync(ConnectCloseWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> OnClientConnectedWebChatAsync(ConnectOpenWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PongClientWebChatAsync(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> InitWebChatAsync(InitWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> GetStateWebChatAsync(GetStateWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> StateWebChatSetAsync(StateWebChatModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> StateWebChatEchoAsync(StateWebChatModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> ShowToastAsync(ShowToastEventModel req, CancellationToken cancellationToken = default);
}