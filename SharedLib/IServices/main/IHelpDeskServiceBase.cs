////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHelpDeskServiceBase
/// </summary>
public interface IHelpDeskServiceBase
{
    /// <summary>
    /// Получить обращения
    /// </summary>
    public Task<TPaginationResponseStandardModel<IssueHelpDeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestStandardModel<ConsoleIssuesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Журнал событий в обращении
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>> PulseJournalSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<UserIssueModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить обращения для пользователя
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>>> IssuesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Создать обращение
    /// </summary>
    public Task<TResponseModel<int>> IssueCreateOrUpdateAsync(TAuthRequestStandardModel<UniversalUpdateRequestModel> issue, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные обращения
    /// </summary>
    public Task<TResponseModel<IssueHelpDeskModelDB[]>> IssuesReadAsync(TAuthRequestStandardModel<IssuesReadRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Подписка на события в обращении (или отписка от событий)
    /// </summary>
    public Task<TResponseModel<bool>> SubscribeUpdateAsync(TAuthRequestStandardModel<SubscribeUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Запрос подписчиков на обращение
    /// </summary>
    public Task<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>> SubscribesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Исполнитель обращения
    /// </summary>
    public Task<TResponseModel<bool>> ExecuterUpdateAsync(TAuthRequestStandardModel<UserIssueModel> req, CancellationToken token = default);

    /// <summary>
    /// Изменить статус обращения
    /// </summary>
    public Task<TResponseModel<bool>> IssueStatusChangeAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);

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