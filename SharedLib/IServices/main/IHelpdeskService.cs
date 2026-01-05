////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// HelpDesk (service)
/// </summary>
public interface IHelpDeskService
{
    /// <summary>
    /// ReplaceTags
    /// </summary>
    public static string ReplaceTags(string documentName, DateTime dateCreated, int documentId, StatusesDocumentsEnum? stepIssue, string raw, string? clearBaseUri, string aboutDocument, bool clearMd = false, string documentPagePath = "issue-card")
    {
        return raw.Replace(GlobalStaticConstants.DocumentNameProperty, documentName)
        .Replace(GlobalStaticConstants.DocumentDateProperty, $"{dateCreated.GetCustomTime().ToString("d", GlobalStaticConstants.RU)} {dateCreated.GetCustomTime().ToString("t", GlobalStaticConstants.RU)}")
        .Replace(GlobalStaticConstants.DocumentStatusProperty, stepIssue?.DescriptionInfo())
        .Replace(GlobalStaticConstants.DocumentLinkProperty, clearMd ? $"{clearBaseUri}/{documentPagePath}/{documentId}" : $"<a href='{clearBaseUri}/{documentPagePath}/{documentId}'>{aboutDocument}</a>")
        .Replace(GlobalStaticConstants.HostAddressProperty, clearMd ? clearBaseUri : $"<a href='{clearBaseUri}'>{clearBaseUri}</a>");
    }

    #region messages
    /// <summary>
    /// Сообщение в обращение
    /// </summary>
    public Task<TResponseModel<int?>> MessageUpdateOrCreateAsync(TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel> req, CancellationToken token = default);

    /// <summary>
    /// Message vote
    /// </summary>
    public Task<TResponseModel<bool?>> MessageVoteAsync(TAuthRequestStandardModel<VoteIssueRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// MessagesList
    /// </summary>
    public Task<TResponseModel<IssueMessageHelpDeskModelDB[]>> MessagesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
    #endregion

    #region issues
    /// <summary>
    /// SubscribesList
    /// </summary>
    public Task<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>> SubscribesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// IssuesSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>>> IssuesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// ConsoleIssuesSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<IssueHelpDeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestStandardModel<ConsoleIssuesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Subscribe update - of context user
    /// </summary>
    public Task<TResponseModel<bool>> ExecuterUpdateAsync(TAuthRequestStandardModel<UserIssueModel> req, CancellationToken token = default);

    /// <summary>
    /// Create (or update) Issue: Рубрика, тема и описание
    /// </summary>
    public Task<TResponseModel<int>> IssueCreateOrUpdateAsync(TAuthRequestStandardModel<UniversalUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Subscribe update - of context user
    /// </summary>
    public Task<TResponseModel<bool?>> SubscribeUpdateAsync(TAuthRequestStandardModel<SubscribeUpdateRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Status change
    /// </summary>
    public Task<TResponseModel<bool>> IssueStatusChangeAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Read issue - of context user
    /// </summary>
    public Task<TResponseModel<IssueHelpDeskModelDB[]>> IssuesReadAsync(TAuthRequestStandardModel<IssuesReadRequestModel> req, CancellationToken token = default);
    #endregion

    #region pulse
    /// <summary>
    /// PulseJournalSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>> PulseJournalSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<UserIssueModel>> req, CancellationToken token = default);

    /// <summary>
    /// Регистрация события из обращения (логи).
    /// </summary>
    /// <remarks>
    /// Плюс рассылка уведомлений участникам события.
    /// </remarks>
    public Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, CancellationToken token = default);

    #endregion

    /// <summary>
    /// Обработка входящего Telegram сообщения
    /// </summary>
    /// <remarks>
    /// Если это ответ в контексте заявки, тогда оно регистрируется и переправляется
    /// </remarks>
    public Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default);

    /// <summary>
    /// SetWebConfig
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigAsync(HelpDeskConfigModel req, CancellationToken token = default);

    /// <summary>
    /// Очистить кеш сегмента консоли
    /// </summary>
    public Task ConsoleSegmentCacheEmptyAsync(StatusesDocumentsEnum? Status = null, CancellationToken token = default);
}