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
    public async Task<ResponseBaseModel> RubricsForArticleSetAsync(RubricsSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForArticleSetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ArticleModelDB>> ArticlesSelectAsync(TPaginationRequestStandardModel<SelectArticlesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ArticleModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesSelectHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> ArticleCreateOrUpdateAsync(ArticleModelDB article, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticleUpdateHelpDeskReceive, article, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ArticleModelDB[]>> ArticlesReadAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ArticleModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ArticlesReadReceive, req, token: token) ?? new();
    #endregion

    #region issue
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> IssueCreateOrUpdateAsync(TAuthRequestStandardModel<UniversalUpdateRequestModel> issue, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssueUpdateHelpDeskReceive, issue, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>>> IssuesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssuesSelectHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueHelpDeskModelDB[]>> IssuesReadAsync(TAuthRequestStandardModel<IssuesReadRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueHelpDeskModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.IssuesGetHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> SubscribeUpdateAsync(TAuthRequestStandardModel<SubscribeUpdateRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.SubscribeIssueUpdateHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>> SubscribesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<SubscriberIssueHelpDeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.SubscribesIssueListHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ExecuterUpdateAsync(TAuthRequestStandardModel<UserIssueModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.ExecuterIssueUpdateHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusChangeAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeIssueHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> PulsePushAsync(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.PulseIssuePushHelpDeskReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>> PulseSelectJournalAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<UserIssueModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.PulseJournalHelpDeskSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<IssueHelpDeskModel>> ConsoleIssuesSelectAsync(TPaginationRequestStandardModel<ConsoleIssuesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<IssueHelpDeskModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ConsoleIssuesSelectHelpDeskReceive, req, token: token) ?? new();
    #endregion

    #region message
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> MessageCreateOrUpdateAsync(TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueUpdateHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> MessageVoteAsync(TAuthRequestStandardModel<VoteIssueRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueVoteHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueMessageHelpDeskModelDB[]>> MessagesListAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueMessageHelpDeskModelDB[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesOfIssueListHelpDeskReceive, req, token: token) ?? new();
    #endregion
}