////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class HelpDeskTransmission(IRabbitClient rabbitClient) : IHelpDeskTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.IncomingTelegramMessageHelpDeskReceive, req, token: token) ?? new();

    #region articles
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRubricsForArticleAsync(ArticleRubricsSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForArticleSetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ArticleModelDB>> ArticlesSelectAsync(TPaginationRequestModel<SelectArticlesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ArticleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesSelectHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> ArticleCreateOrUpdateAsync(ArticleModelDB article, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticleUpdateHelpDeskReceive, article, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ArticleModelDB[]>> ArticlesReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ArticleModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesReadReceive, req, token: token) ?? new();
    #endregion

    #region rubric
    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpDeskModelDB>>> RubricsGetAsync(IEnumerable<int> rubricsIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpDeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesGetHelpDeskReceive, rubricsIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpDeskModelDB>>> RubricReadAsync(int rubricId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpDeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesReadHelpDeskReceive, rubricId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricIssueHelpDeskModelDB issueTheme, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateReceive, issueTheme, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<UniversalBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesListHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesMoveHelpDeskReceive, req, token: token) ?? new();
    #endregion

    #region issue
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> IssueCreateOrUpdateAsync(TAuthRequestModel<UniversalUpdateRequestModel> issue, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssueUpdateHelpDeskReceive, issue, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>> IssuesSelectAsync(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssuesSelectHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueHelpDeskModelDB[]>> IssuesReadAsync(TAuthRequestModel<IssuesReadRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueHelpDeskModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssuesGetHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> SubscribeUpdateAsync(TAuthRequestModel<SubscribeUpdateRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.SubscribeIssueUpdateHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>> SubscribesListAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.SubscribesIssueListHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ExecuterUpdateAsync(TAuthRequestModel<UserIssueModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.ExecuterIssueUpdateHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusChangeAsync(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeIssueHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.PulseIssuePushHelpDeskReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>> PulseSelectJournalAsync(TAuthRequestModel<TPaginationRequestModel<UserIssueModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<PulseViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.PulseJournalHelpDeskSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IssueHelpDeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestModel<ConsoleIssuesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<IssueHelpDeskModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ConsoleIssuesSelectHelpDeskReceive, req, token: token) ?? new();
    #endregion

    #region message
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> MessageCreateOrUpdateAsync(TAuthRequestModel<IssueMessageHelpDeskBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueUpdateHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> MessageVoteAsync(TAuthRequestModel<VoteIssueRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueVoteHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueMessageHelpDeskModelDB[]>> MessagesListAsync(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueMessageHelpDeskModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesOfIssueListHelpDeskReceive, req, token: token) ?? new();
    #endregion
}