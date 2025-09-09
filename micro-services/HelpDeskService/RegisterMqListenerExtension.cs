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
            .RegisterMqListener<RubricCreateOrUpdateReceive, RubricStandardModel, TResponseModel<int>>()
            .RegisterMqListener<RubricMoveReceive, TAuthRequestModel<RowMoveModel>, ResponseBaseModel>()
            .RegisterMqListener<RubricReadReceive, int, TResponseModel<List<RubricStandardModel>>>()
            .RegisterMqListener<RubricsGetReceive, int[], TResponseModel<List<RubricStandardModel>>>()

            .RegisterMqListener<IssuesSelectReceive, TAuthRequestModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>>, TResponseModel<TPaginationResponseModel<IssueHelpDeskModel>>>()
            .RegisterMqListener<ArticlesSelectReceive, TPaginationRequestStandardModel<SelectArticlesRequestModel>, TPaginationResponseModel<ArticleModelDB>>()
            .RegisterMqListener<IssueCreateOrUpdateReceive, TAuthRequestModel<UniversalUpdateRequestModel>, TResponseModel<int>>()
            .RegisterMqListener<MessageVoteReceive, TAuthRequestModel<VoteIssueRequestModel>, TResponseModel<bool?>>()
            .RegisterMqListener<MessageUpdateOrCreateReceive, TAuthRequestModel<IssueMessageHelpDeskBaseModel>, TResponseModel<int?>>()
            .RegisterMqListener<SetWebConfigReceive, HelpDeskConfigModel, ResponseBaseModel>()
            .RegisterMqListener<UpdateRubricsForArticleReceive, ArticleRubricsSetModel, ResponseBaseModel>()
            .RegisterMqListener<ArticlesReadReceive, int[], TResponseModel<ArticleModelDB[]>>()
            .RegisterMqListener<ArticleCreateOrUpdateReceive, ArticleModelDB, TResponseModel<int>>()
            .RegisterMqListener<IssuesReadReceive, TAuthRequestModel<IssuesReadRequestModel>, TResponseModel<IssueHelpDeskModelDB[]>>()
            .RegisterMqListener<SubscribeUpdateReceive, TAuthRequestModel<SubscribeUpdateRequestModel>, TResponseModel<bool?>>()
            .RegisterMqListener<SubscribesListReceive, TAuthRequestModel<int>, TResponseModel<List<SubscriberIssueHelpDeskModelDB>>>()
            .RegisterMqListener<ExecuterUpdateReceive, TAuthRequestModel<UserIssueModel>, TResponseModel<bool>>()
            .RegisterMqListener<MessagesListReceive, TAuthRequestModel<int>, TResponseModel<IssueMessageHelpDeskModelDB[]>>()
            .RegisterMqListener<StatusChangeReceive, TAuthRequestModel<StatusChangeRequestModel>, TResponseModel<bool>>()
            .RegisterMqListener<PulseIssueReceive, PulseRequestModel, TResponseModel<bool>>()
            .RegisterMqListener<PulseJournalSelectReceive, TAuthRequestModel<TPaginationRequestStandardModel<UserIssueModel>>, TResponseModel<TPaginationResponseModel<PulseViewModel>>>()
            .RegisterMqListener<TelegramMessageIncomingReceive, TelegramIncomingMessageModel, ResponseBaseModel>()
            .RegisterMqListener<ConsoleIssuesSelectReceive, TPaginationRequestStandardModel<ConsoleIssuesRequestModel>, TPaginationResponseModel<IssueHelpDeskModel>>()
            ;
    }
}