////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.helpdesk;

namespace HelpDeskService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection HelpDeskRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<RubricsListReceive, RubricsListRequestModel, List<UniversalBaseModel>>()
            .RegisterMqListener<RubricCreateOrUpdateReceive, RubricIssueHelpDeskModelDB, TResponseModel<int>>()
            .RegisterMqListener<IssuesSelectReceive, TAuthRequestModel<TPaginationRequestModel<SelectIssuesRequestModel>>, TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>>()
            .RegisterMqListener<ArticlesSelectReceive, TPaginationRequestModel<SelectArticlesRequestModel>, TPaginationResponseModel<ArticleModelDB>>()
            .RegisterMqListener<IssueCreateOrUpdateReceive, TAuthRequestModel<UniversalUpdateRequestModel>, TResponseModel<int>>()
            .RegisterMqListener<MessageVoteReceive, TAuthRequestModel<VoteIssueRequestModel>, TResponseModel<bool?>>()
            .RegisterMqListener<MessageUpdateOrCreateReceive, TAuthRequestModel<IssueMessageHelpDeskBaseModel>, TResponseModel<int?>>()
            .RegisterMqListener<RubricMoveReceive, TAuthRequestModel<RowMoveModel>, ResponseBaseModel>()
            .RegisterMqListener<SetWebConfigReceive, HelpDeskConfigModel, ResponseBaseModel>()
            .RegisterMqListener<UpdateRubricsForArticleReceive, ArticleRubricsSetModel, ResponseBaseModel>()
            .RegisterMqListener<ArticlesReadReceive, int[], TResponseModel<ArticleModelDB[]>>()
            .RegisterMqListener<ArticleCreateOrUpdateReceive, ArticleModelDB, TResponseModel<int>>()
            .RegisterMqListener<IssuesReadReceive, TAuthRequestModel<IssuesReadRequestModel>, TResponseModel<IssueHelpDeskModelDB[]>>()
            .RegisterMqListener<RubricReadReceive, int, TResponseModel<List<RubricIssueHelpDeskModelDB>>>()
            .RegisterMqListener<RubricsGetReceive, int[], TResponseModel<List<RubricIssueHelpDeskModelDB>>>()
            .RegisterMqListener<SubscribeUpdateReceive, TAuthRequestModel<SubscribeUpdateRequestModel>, TResponseModel<bool?>>()
            .RegisterMqListener<SubscribesListReceive, TAuthRequestModel<int>, TResponseModel<List<SubscriberIssueHelpDeskModelDB>>>()
            .RegisterMqListener<ExecuterUpdateReceive, TAuthRequestModel<UserIssueModel>, TResponseModel<bool>>()
            .RegisterMqListener<MessagesListReceive, TAuthRequestModel<int>, TResponseModel<IssueMessageHelpDeskModelDB[]>>()
            .RegisterMqListener<StatusChangeReceive, TAuthRequestModel<StatusChangeRequestModel>, TResponseModel<bool>>()
            .RegisterMqListener<PulseIssueReceive, PulseRequestModel, TResponseModel<bool>>()
            .RegisterMqListener<PulseJournalSelectReceive, TAuthRequestModel<TPaginationRequestModel<UserIssueModel>>, TResponseModel<TPaginationResponseModel<PulseViewModel>>>()
            .RegisterMqListener<TelegramMessageIncomingReceive, TelegramIncomingMessageModel, ResponseBaseModel>()
            .RegisterMqListener<ConsoleIssuesSelectReceive, TPaginationRequestModel<ConsoleIssuesRequestModel>, TPaginationResponseModel<IssueHelpDeskModel>>()
            ;
    }
}