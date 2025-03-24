﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class HelpdeskTransmission(IRabbitClient rabbitClient) : IHelpdeskTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramMessageIncoming(TelegramIncomingMessageModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.IncomingTelegramMessageHelpdeskReceive, req, token: token) ?? new();

    #region articles
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRubricsForArticle(ArticleRubricsSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.RubricsForArticleSetReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ArticleModelDB>> ArticlesSelect(TPaginationRequestModel<SelectArticlesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ArticleModelDB>>(GlobalStaticConstants.TransmissionQueues.ArticlesSelectHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> ArticleCreateOrUpdate(ArticleModelDB article, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.ArticleUpdateHelpdeskReceive, article, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<ArticleModelDB[]>> ArticlesRead(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<ArticleModelDB[]>>(GlobalStaticConstants.TransmissionQueues.ArticlesReadReceive, req, token: token) ?? new();
    #endregion

    #region rubric
    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricsGet(IEnumerable<int> rubricsIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpdeskModelDB>>>(GlobalStaticConstants.TransmissionQueues.RubricsForIssuesGetHelpdeskReceive, rubricsIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpdeskModelDB>>> RubricRead(int rubricId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpdeskModelDB>>>(GlobalStaticConstants.TransmissionQueues.RubricForIssuesReadHelpdeskReceive, rubricId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RubricCreateOrUpdate(RubricIssueHelpdeskModelDB issueTheme, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.RubricForIssuesUpdateHelpdeskReceive, issueTheme, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> RubricsList(RubricsListRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<UniversalBaseModel>>(GlobalStaticConstants.TransmissionQueues.RubricsForIssuesListHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricMove(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.RubricForIssuesMoveHelpdeskReceive, req, token: token) ?? new();
    #endregion

    #region issue
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> IssueCreateOrUpdate(TAuthRequestModel<UniversalUpdateRequestModel> issue, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.IssueUpdateHelpdeskReceive, issue, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>> IssuesSelect(TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<IssueHelpdeskModel>>>(GlobalStaticConstants.TransmissionQueues.IssuesSelectHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueHelpdeskModelDB[]>> IssuesRead(TAuthRequestModel<IssuesReadRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueHelpdeskModelDB[]>>(GlobalStaticConstants.TransmissionQueues.IssuesGetHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> SubscribeUpdate(TAuthRequestModel<SubscribeUpdateRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.SubscribeIssueUpdateHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>> SubscribesList(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<SubscriberIssueHelpdeskModelDB>>>(GlobalStaticConstants.TransmissionQueues.SubscribesIssueListHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> ExecuterUpdate(TAuthRequestModel<UserIssueModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.ExecuterIssueUpdateHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> StatusChange(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.StatusChangeIssueHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> PulsePush(PulseRequestModel req, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.PulseIssuePushHelpdeskReceive, req, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<PulseViewModel>>> PulseSelectJournal(TAuthRequestModel<TPaginationRequestModel<UserIssueModel>> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<TPaginationResponseModel<PulseViewModel>>>(GlobalStaticConstants.TransmissionQueues.PulseJournalHelpdeskSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IssueHelpdeskModel>> ConsoleIssuesSelect(TPaginationRequestModel<ConsoleIssuesRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<IssueHelpdeskModel>>(GlobalStaticConstants.TransmissionQueues.ConsoleIssuesSelectHelpdeskReceive, req, token: token) ?? new();
    #endregion

    #region message
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> MessageCreateOrUpdate(TAuthRequestModel<IssueMessageHelpdeskBaseModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.MessageOfIssueUpdateHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> MessageVote(TAuthRequestModel<VoteIssueRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.MessageOfIssueVoteHelpdeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<IssueMessageHelpdeskModelDB[]>> MessagesList(TAuthRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<IssueMessageHelpdeskModelDB[]>>(GlobalStaticConstants.TransmissionQueues.MessagesOfIssueListHelpdeskReceive, req, token: token) ?? new();
    #endregion
}