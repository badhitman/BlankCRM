////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class HelpdeskTransmission(IRabbitClient rabbitClient) : IHelpdeskTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.IncomingTelegramMessageHelpdeskReceive, req, token: token) ?? new();

    #region articles
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRubricsForArticleAsync(ArticleRubricsSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForArticleSetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ArticleModelDB>> ArticlesSelectAsync(TPaginationRequestModel<SelectArticlesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ArticleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesSelectHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> ArticleCreateOrUpdateAsync(ArticleModelDB article, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticleUpdateHelpdeskReceive, article, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ArticleModelDB[]>> ArticlesReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ArticleModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesReadReceive, req, token: token) ?? new();
    #endregion

    #region rubric
    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricsGetAsync(IEnumerable<int> rubricsIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpdeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesGetHelpdeskReceive, rubricsIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricReadAsync(int rubricId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpdeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesReadHelpdeskReceive, rubricId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricIssueHelpdeskModelDB issueTheme, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateHelpdeskReceive, issueTheme, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<UniversalBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesListHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesMoveHelpdeskReceive, req, token: token) ?? new();
    #endregion

    #region issue
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> IssueCreateOrUpdateAsync(TAuthRequestModel<UniversalUpdateRequestModel> issue, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssueUpdateHelpdeskReceive, issue, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>> IssuesSelectAsync(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssuesSelectHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueHelpdeskModelDB[]>> IssuesReadAsync(TAuthRequestModel<IssuesReadRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueHelpdeskModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssuesGetHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> SubscribeUpdateAsync(TAuthRequestModel<SubscribeUpdateRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.SubscribeIssueUpdateHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>> SubscribesListAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.SubscribesIssueListHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ExecuterUpdateAsync(TAuthRequestModel<UserIssueModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.ExecuterIssueUpdateHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusChangeAsync(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeIssueHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.PulseIssuePushHelpdeskReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>> PulseSelectJournalAsync(TAuthRequestModel<TPaginationRequestModel<UserIssueModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<PulseViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.PulseJournalHelpdeskSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IssueHelpdeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestModel<ConsoleIssuesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<IssueHelpdeskModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ConsoleIssuesSelectHelpdeskReceive, req, token: token) ?? new();
    #endregion

    #region message
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> MessageCreateOrUpdateAsync(TAuthRequestModel<IssueMessageHelpdeskBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueUpdateHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> MessageVoteAsync(TAuthRequestModel<VoteIssueRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueVoteHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueMessageHelpdeskModelDB[]>> MessagesListAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueMessageHelpdeskModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesOfIssueListHelpdeskReceive, req, token: token) ?? new();
    #endregion
}