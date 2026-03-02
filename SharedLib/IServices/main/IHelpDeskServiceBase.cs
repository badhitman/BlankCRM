////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHelpDeskServiceBase
/// </summary>
public interface IHelpDeskServiceBase
{
    #region message
    /// <summary>
    /// Добавить сообщение к обращению
    /// </summary>
    public Task<TResponseModel<int>> MessageCreateOrUpdateAsync(TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Сообщения из обращения
    /// </summary>
    public Task<TResponseModel<IssueMessageHelpDeskModelDB[]>> MessagesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Обработка входящего Telegram сообщения
    /// </summary>
    /// <remarks>
    /// Если это ответ в контексте заявки, тогда оно регистрируется и переправляется
    /// </remarks>
    public Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default);

    /// <summary>
    /// Сообщение из обращения помечается как ответ (либо этот признак снимается: в зависимости от запроса)
    /// </summary>
    public Task<TResponseModel<bool>> MessageVoteAsync(TAuthRequestStandardModel<VoteIssueRequestModel> req, CancellationToken token = default);
    #endregion
}