////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IHelpDeskTransmission
/// </summary>
public interface IHelpDeskTransmission
{
    /// <summary>
    /// Входящее сообщение от клиента в TelegramBot
    /// </summary>
    public Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default);

    #region articles
    /// <summary>
    /// UpdateRubricsForArticle
    /// </summary>
    public Task<ResponseBaseModel> UpdateRubricsForArticleAsync(ArticleRubricsSetModel req, CancellationToken token = default);

    /// <summary>
    /// Получить статьи 
    /// </summary>
    public Task<TResponseModel<ArticleModelDB[]>> ArticlesReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Подобрать статьи 
    /// </summary>
    public Task<TPaginationResponseModel<ArticleModelDB>> ArticlesSelectAsync(TPaginationRequestModel<SelectArticlesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать (обновить) статью
    /// </summary>
    public Task<TResponseModel<int>> ArticleCreateOrUpdateAsync(ArticleModelDB article, CancellationToken token = default);
    #endregion

    #region issue
    /// <summary>
    /// Получить обращения для пользователя
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>> IssuesSelectAsync(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Создать обращение
    /// </summary>
    public Task<TResponseModel<int>> IssueCreateOrUpdateAsync(TAuthRequestModel<UniversalUpdateRequestModel> issue, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные обращения
    /// </summary>
    public Task<TResponseModel<IssueHelpDeskModelDB[]>> IssuesReadAsync(TAuthRequestModel<IssuesReadRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Подписка на события в обращении (или отписка от событий)
    /// </summary>
    public Task<TResponseModel<bool>> SubscribeUpdateAsync(TAuthRequestModel<SubscribeUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Запрос подписчиков на обращение
    /// </summary>
    public Task<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>> SubscribesListAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Исполнитель обращения
    /// </summary>
    public Task<TResponseModel<bool>> ExecuterUpdateAsync(TAuthRequestModel<UserIssueModel> req, CancellationToken token = default);

    /// <summary>
    /// Изменить статус обращения
    /// </summary>
    public Task<TResponseModel<bool>> StatusChangeAsync(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Добавить событие в журнал
    /// </summary>
    public Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Журнал событий в обращении
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>> PulseSelectJournalAsync(TAuthRequestModel<TPaginationRequestModel<UserIssueModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить обращения
    /// </summary>
    public Task<TPaginationResponseModel<IssueHelpDeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestModel<ConsoleIssuesRequestModel> req, CancellationToken token = default);
    #endregion

    #region message
    /// <summary>
    /// Сообщение из обращения помечается как ответ (либо этот признак снимается: в зависимости от запроса)
    /// </summary>
    public Task<TResponseModel<bool>> MessageVoteAsync(TAuthRequestModel<VoteIssueRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Добавить сообщение к обращению
    /// </summary>
    public Task<TResponseModel<int>> MessageCreateOrUpdateAsync(TAuthRequestModel<IssueMessageHelpDeskBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Сообщения из обращения
    /// </summary>
    public Task<TResponseModel<IssueMessageHelpDeskModelDB[]>> MessagesListAsync(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion
}