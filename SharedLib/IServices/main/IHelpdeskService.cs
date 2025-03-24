////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Helpdesk (service)
/// </summary>
public interface IHelpdeskService
{
    /// <summary>
    /// ReplaceTags
    /// </summary>
    public static string ReplaceTags(string documentName, DateTime dateCreated, int documentId, StatusesDocumentsEnum stepIssue, string raw, string? clearBaseUri, string aboutDocument, bool clearMd = false, string documentPagePath = "issue-card")
    {
        return raw.Replace(GlobalStaticConstants.DocumentNameProperty, documentName)
        .Replace(GlobalStaticConstants.DocumentDateProperty, $"{dateCreated.GetCustomTime().ToString("d", GlobalStaticConstants.RU)} {dateCreated.GetCustomTime().ToString("t", GlobalStaticConstants.RU)}")
        .Replace(GlobalStaticConstants.DocumentStatusProperty, stepIssue.DescriptionInfo())
        .Replace(GlobalStaticConstants.DocumentLinkAddressProperty, clearMd ? $"{clearBaseUri}/{documentPagePath}/{documentId}" : $"<a href='{clearBaseUri}/{documentPagePath}/{documentId}'>{aboutDocument}</a>")
        .Replace(GlobalStaticConstants.HostAddressProperty, clearMd ? clearBaseUri : $"<a href='{clearBaseUri}'>{clearBaseUri}</a>");
    }

    #region messages
    /// <summary>
    /// Сообщение в обращение
    /// </summary>
    public Task<TResponseModel<int?>> MessageUpdateOrCreate(TAuthRequestModel<IssueMessageHelpdeskBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Message vote
    /// </summary>
    public Task<TResponseModel<bool?>> MessageVote(TAuthRequestModel<VoteIssueRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// MessagesList
    /// </summary>
    public Task<TResponseModel<IssueMessageHelpdeskModelDB[]>> MessagesList(TAuthRequestModel<int> req, CancellationToken token = default);
    #endregion

    #region issues
    /// <summary>
    /// SubscribesList
    /// </summary>
    public Task<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>> SubscribesList(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// IssuesSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>> IssuesSelect(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// ConsoleIssuesSelect
    /// </summary>
    public Task<TPaginationResponseModel<IssueHelpdeskModel>> ConsoleIssuesSelect(TPaginationRequestModel<ConsoleIssuesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Subscribe update - of context user
    /// </summary>
    public Task<TResponseModel<bool>> ExecuterUpdate(TAuthRequestModel<UserIssueModel> req, CancellationToken token = default);

    /// <summary>
    /// Create (or update) Issue: Рубрика, тема и описание
    /// </summary>
    public Task<TResponseModel<int>> IssueCreateOrUpdate(TAuthRequestModel<UniversalUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Subscribe update - of context user
    /// </summary>
    public Task<TResponseModel<bool?>> SubscribeUpdate(TAuthRequestModel<SubscribeUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Status change
    /// </summary>
    public Task<TResponseModel<bool>> IssueStatusChange(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Read issue - of context user
    /// </summary>
    public Task<TResponseModel<IssueHelpdeskModelDB[]>> IssuesRead(TAuthRequestModel<IssuesReadRequestModel> req, CancellationToken token = default);
    #endregion

    #region rubric
    /// <summary>
    /// Получить рубрики, вложенные в рубрику (если не указано, то root перечень)
    /// </summary>
    public Task<List<UniversalBaseModel>> RubricsList(RubricsListRequestModel req, CancellationToken token = default);

    /// <summary>
    /// RubricMove
    /// </summary>
    public Task<ResponseBaseModel> RubricMove(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default);

    /// <summary>
    /// Rubric create (or update)
    /// </summary>
    public Task<TResponseModel<int>> RubricCreateOrUpdate(RubricIssueHelpdeskModelDB req, CancellationToken token = default);

    /// <summary>
    /// Rubric read
    /// </summary>
    public Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricRead(int rubricId, CancellationToken token = default);

    /// <summary>
    /// Rubrics get
    /// </summary>
    public Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricsGet(int[] rubricsIds, CancellationToken token = default);
    #endregion

    #region pulse
    /// <summary>
    /// PulseJournalSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>> PulseJournalSelect(TAuthRequestModel<TPaginationRequestModel<UserIssueModel>> req, CancellationToken token = default);

    /// <summary>
    /// Регистрация события из обращения (логи).
    /// </summary>
    /// <remarks>
    /// Плюс рассылка уведомлений участникам события.
    /// </remarks>
    public Task<TResponseModel<bool>> PulsePush(PulseRequestModel req, CancellationToken token = default);

    #endregion

    /// <summary>
    /// Обработка входящего Telegram сообщения
    /// </summary>
    /// <remarks>
    /// Если это ответ в контексте заявки, тогда оно регистрируется и переправляется
    /// </remarks>
    public Task<ResponseBaseModel> TelegramMessageIncoming(TelegramIncomingMessageModel req, CancellationToken token = default);

    /// <summary>
    /// SetWebConfig
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfig(HelpdeskConfigModel req, CancellationToken token = default);

    /// <summary>
    /// Очистить кеш сегмента консоли
    /// </summary>
    public Task ConsoleSegmentCacheEmpty(StatusesDocumentsEnum? Status = null, CancellationToken token = default);
}