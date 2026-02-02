////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IWebChatService
/// </summary>
public interface IWebChatService : IAsyncDisposable
{
    /// <inheritdoc/>
    public Task<TResponseModel<InitWebChatSessionResponseModel>> InitWebChatSessionAsync(InitWebChatSessionRequestModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<MessageWebChatModelDB>> SelectMessagesWebChatAsync(TPaginationRequestStandardModel<SelectMessagesForWebChatRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateMessageWebChatAsync(MessageWebChatModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateMessageWebChatAsync(TAuthRequestStandardModel<MessageWebChatModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteToggleMessageWebChatAsync(TAuthRequestStandardModel<DeleteToggleMessageWebChatRequestModel> req, CancellationToken token = default);
}