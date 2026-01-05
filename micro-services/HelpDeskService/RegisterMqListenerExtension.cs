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
            .RegisterMqListener<RubricsListReceive, RubricsListRequestStandardModel, List<UniversalBaseModel>>()
            .RegisterMqListener<RubricCreateOrUpdateReceive, RubricStandardModel, TResponseModel<int>>()
            .RegisterMqListener<RubricMoveReceive, TAuthRequestStandardModel<RowMoveModel>, ResponseBaseModel>()
            .RegisterMqListener<RubricReadReceive, int, TResponseModel<List<RubricStandardModel>>>()
            .RegisterMqListener<RubricsGetReceive, int[], TResponseModel<List<RubricStandardModel>>>()

            .RegisterMqListener<IssuesSelectReceive, TAuthRequestStandardModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>>, TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>>>()
            .RegisterMqListener<ArticlesSelectReceive, TPaginationRequestStandardModel<SelectArticlesRequestModel>, TPaginationResponseStandardModel<ArticleModelDB>>()
            .RegisterMqListener<IssueCreateOrUpdateReceive, TAuthRequestStandardModel<UniversalUpdateRequestModel>, TResponseModel<int>>()
            .RegisterMqListener<MessageVoteReceive, TAuthRequestStandardModel<VoteIssueRequestModel>, TResponseModel<bool?>>()
            .RegisterMqListener<MessageUpdateOrCreateReceive, TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel>, TResponseModel<int?>>()
            .RegisterMqListener<SetWebConfigReceive, HelpDeskConfigModel, ResponseBaseModel>()
            .RegisterMqListener<UpdateRubricsForArticleReceive, ArticleRubricsSetModel, ResponseBaseModel>()
            .RegisterMqListener<ArticlesReadReceive, int[], TResponseModel<ArticleModelDB[]>>()
            .RegisterMqListener<ArticleCreateOrUpdateReceive, ArticleModelDB, TResponseModel<int>>()
            .RegisterMqListener<IssuesReadReceive, TAuthRequestStandardModel<IssuesReadRequestModel>, TResponseModel<IssueHelpDeskModelDB[]>>()
            .RegisterMqListener<SubscribeUpdateReceive, TAuthRequestStandardModel<SubscribeUpdateRequestModel>, TResponseModel<bool?>>()
            .RegisterMqListener<SubscribesListReceive, TAuthRequestStandardModel<int>, TResponseModel<List<SubscriberIssueHelpDeskModelDB>>>()
            .RegisterMqListener<ExecuterUpdateReceive, TAuthRequestStandardModel<UserIssueModel>, TResponseModel<bool>>()
            .RegisterMqListener<MessagesListReceive, TAuthRequestStandardModel<int>, TResponseModel<IssueMessageHelpDeskModelDB[]>>()
            .RegisterMqListener<StatusChangeReceive, TAuthRequestStandardModel<StatusChangeRequestModel>, TResponseModel<bool>>()
            .RegisterMqListener<PulseIssueReceive, PulseRequestModel, TResponseModel<bool>>()
            .RegisterMqListener<PulseJournalSelectReceive, TAuthRequestStandardModel<TPaginationRequestStandardModel<UserIssueModel>>, TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>>()
            .RegisterMqListener<TelegramMessageIncomingReceive, TelegramIncomingMessageModel, ResponseBaseModel>()
            .RegisterMqListener<ConsoleIssuesSelectReceive, TPaginationRequestStandardModel<ConsoleIssuesRequestModel>, TPaginationResponseStandardModel<IssueHelpDeskModel>>()
            ;
    }
}