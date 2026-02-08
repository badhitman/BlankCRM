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
    public Task<ResponseBaseModel> ClientDisconnectedWebChatAsync(ConnectionCloseWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> OnClientConnectedWebChatAsync(ConnectionOpenWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> PongClientWebChatAsync(PongClientsWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> InitWebChatAsync(InitWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> StateGetWebChatAsync(GetStateWebChatEventModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> StateSetWebChatAsync(StateWebChatModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> StateEchoWebChatAsync(StateWebChatModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> ToastShowAsync(ShowToastEventModel req, CancellationToken cancellationToken = default);
}