////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHelpDeskTransmission
/// </summary>
public interface IHelpDeskTransmission : IArticlesService
{
    /// <summary>
    /// Входящее сообщение от клиента в TelegramBot
    /// </summary>
    public Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default);

    #region issue
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
    public Task<TResponseModel<bool>> StatusChangeAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Добавить событие в журнал
    /// </summary>
    public Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Журнал событий в обращении
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>> PulseSelectJournalAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<UserIssueModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить обращения
    /// </summary>
    public Task<TPaginationResponseStandardModel<IssueHelpDeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestStandardModel<ConsoleIssuesRequestModel> req, CancellationToken token = default);
    #endregion

    #region message
    /// <summary>
    /// Сообщение из обращения помечается как ответ (либо этот признак снимается: в зависимости от запроса)
    /// </summary>
    public Task<TResponseModel<bool>> MessageVoteAsync(TAuthRequestStandardModel<VoteIssueRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Добавить сообщение к обращению
    /// </summary>
    public Task<TResponseModel<int>> MessageCreateOrUpdateAsync(TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Сообщения из обращения
    /// </summary>
    public Task<TResponseModel<IssueMessageHelpDeskModelDB[]>> MessagesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion
}