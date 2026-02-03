////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IWebChatService
/// </summary>
public interface IWebChatService : IAsyncDisposable
{
    #region messages
    /// <inheritdoc/>
    public Task<TResponseModel<InitWebChatSessionResponseModel>> InitWebChatSessionAsync(InitWebChatSessionRequestModel req, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<TResponseModel<SelectMessagesForWebChatResponseModel>> SelectMessagesWebChatAsync(SelectMessagesForWebChatRequestModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<int>> CreateMessageWebChatAsync(MessageWebChatModelDB req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateMessageWebChatAsync(TAuthRequestStandardModel<MessageWebChatModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteToggleMessageWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion

    #region messages dialogs    
    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<DialogWebChatViewModel>> SelectDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> UpdateDialogWebChatAsync(TAuthRequestStandardModel<DialogWebChatBaseModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteToggleDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion

    #region users-joins-dialogs
    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>> SelectUsersJoinsDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> CreateUserJoinDialogWebChatAsync(TAuthRequestStandardModel<UserJoinDialogWebChatModelDB> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> DeleteUserJoinDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion
}