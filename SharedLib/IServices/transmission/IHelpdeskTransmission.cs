////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Удалённый вызов команд в Helpdesk службе
/// </summary>
public interface IHelpdeskTransmission
{
    /// <summary>
    /// Входящее сообщение от клиента в TelegramBot
    /// </summary>
    public Task<ResponseBaseModel> TelegramMessageIncoming(TelegramIncomingMessageModel req, CancellationToken token = default);

    #region articles
    /// <summary>
    /// UpdateRubricsForArticle
    /// </summary>
    public Task<ResponseBaseModel> UpdateRubricsForArticle(ArticleRubricsSetModel req, CancellationToken token = default);

    /// <summary>
    /// Получить статьи 
    /// </summary>
    public Task<TResponseModel<ArticleModelDB[]>> ArticlesRead(int[] req, CancellationToken token = default);

    /// <summary>
    /// Подобрать статьи 
    /// </summary>
    public Task<TPaginationResponseModel<ArticleModelDB>> ArticlesSelect(TPaginationRequestModel<SelectArticlesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Создать (обновить) статью
    /// </summary>
    public Task<TResponseModel<int>> ArticleCreateOrUpdate(ArticleModelDB article, CancellationToken token = default);
    #endregion

    #region rubric
    /// <summary>
    /// Получить темы обращений
    /// </summary>
    public Task<List<UniversalBaseModel>> RubricsList(RubricsListRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Создать тему для обращений
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdate(RubricIssueHelpdeskModelDB issueTheme, CancellationToken token = default);

    /// <summary>
    /// Сдвинуть рубрику
    /// </summary>
    public Task<ResponseBaseModel> RubricMove(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные рубрики (вместе со всеми вышестоящими родителями)
    /// </summary>
    public Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricRead(int rubricId, CancellationToken token = default);

    /// <summary>
    /// Получить рубрики
    /// </summary>
    public Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricsGet(IEnumerable<int> rubricsIds, CancellationToken token = default);
    #endregion

    #region issue
    /// <summary>
    /// Получить обращения для пользователя
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>> IssuesSelect(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Создать обращение
    /// </summary>
    public Task<TResponseModel<int>> IssueCreateOrUpdate(TAuthRequestModel<UniversalUpdateRequestModel> issue, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные обращения
    /// </summary>
    public Task<TResponseModel<IssueHelpdeskModelDB[]>> IssuesRead(TAuthRequestModel<IssuesReadRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Подписка на события в обращении (или отписка от событий)
    /// </summary>
    public Task<TResponseModel<bool>> SubscribeUpdate(TAuthRequestModel<SubscribeUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Запрос подписчиков на обращение
    /// </summary>
    public Task<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>> SubscribesList(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Исполнитель обращения
    /// </summary>
    public Task<TResponseModel<bool>> ExecuterUpdate(TAuthRequestModel<UserIssueModel> req, CancellationToken token = default);

    /// <summary>
    /// Изменить статус обращения
    /// </summary>
    public Task<TResponseModel<bool>> StatusChange(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Добавить событие в журнал
    /// </summary>
    public Task<TResponseModel<bool>> PulsePush(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Журнал событий в обращении
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>> PulseSelectJournal(TAuthRequestModel<TPaginationRequestModel<UserIssueModel>> req, CancellationToken token = default);

    /// <summary>
    /// Получить обращения
    /// </summary>
    public Task<TPaginationResponseModel<IssueHelpdeskModel>> ConsoleIssuesSelect(TPaginationRequestModel<ConsoleIssuesRequestModel> req, CancellationToken token = default);
    #endregion

    #region message
    /// <summary>
    /// Сообщение из обращения помечается как ответ (либо этот признак снимается: в зависимости от запроса)
    /// </summary>
    public Task<TResponseModel<bool>> MessageVote(TAuthRequestModel<VoteIssueRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Добавить сообщение к обращению
    /// </summary>
    public Task<TResponseModel<int>> MessageCreateOrUpdate(TAuthRequestModel<IssueMessageHelpdeskBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Сообщения из обращения
    /// </summary>
    public Task<TResponseModel<IssueMessageHelpdeskModelDB[]>> MessagesList(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion
}