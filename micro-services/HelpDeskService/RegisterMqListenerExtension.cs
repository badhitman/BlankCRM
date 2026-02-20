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
            .RegisterListenerRabbitMQ<RubricsListReceive, RubricsListRequestStandardModel, List<RubricNestedModel>>()
            .RegisterListenerRabbitMQ<RubricCreateOrUpdateReceive, RubricStandardModel, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<RubricMoveReceive, TAuthRequestStandardModel<RowMoveModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<RubricReadReceive, int, TResponseModel<List<RubricStandardModel>>>()
            .RegisterListenerRabbitMQ<RubricsGetReceive, int[], TResponseModel<List<RubricStandardModel>>>()

            .RegisterListenerRabbitMQ<IssuesSelectReceive, TAuthRequestStandardModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>>, TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>>>()
            .RegisterListenerRabbitMQ<ArticlesSelectReceive, TPaginationRequestStandardModel<SelectArticlesRequestModel>, TPaginationResponseStandardModel<ArticleModelDB>>()
            .RegisterListenerRabbitMQ<IssueCreateOrUpdateReceive, TAuthRequestStandardModel<UniversalUpdateRequestModel>, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<MessageVoteReceive, TAuthRequestStandardModel<VoteIssueRequestModel>, TResponseModel<bool?>>()
            .RegisterListenerRabbitMQ<MessageUpdateOrCreateReceive, TAuthRequestStandardModel<IssueMessageHelpDeskBaseModel>, TResponseModel<int?>>()
            .RegisterListenerRabbitMQ<SetWebConfigReceive, HelpDeskConfigModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateRubricsForArticleReceive, RubricsSetModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ArticlesReadReceive, int[], TResponseModel<ArticleModelDB[]>>()
            .RegisterListenerRabbitMQ<ArticleCreateOrUpdateReceive, ArticleModelDB, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<IssuesReadReceive, TAuthRequestStandardModel<IssuesReadRequestModel>, TResponseModel<IssueHelpDeskModelDB[]>>()
            .RegisterListenerRabbitMQ<SubscribeUpdateReceive, TAuthRequestStandardModel<SubscribeUpdateRequestModel>, TResponseModel<bool?>>()
            .RegisterListenerRabbitMQ<SubscribesListReceive, TAuthRequestStandardModel<int>, TResponseModel<List<SubscriberIssueHelpDeskModelDB>>>()
            .RegisterListenerRabbitMQ<ExecuterUpdateReceive, TAuthRequestStandardModel<UserIssueModel>, TResponseModel<bool>>()
            .RegisterListenerRabbitMQ<MessagesListReceive, TAuthRequestStandardModel<int>, TResponseModel<IssueMessageHelpDeskModelDB[]>>()
            .RegisterListenerRabbitMQ<StatusIssueChangeReceive, TAuthRequestStandardModel<StatusChangeRequestModel>, TResponseModel<bool>>()
            .RegisterListenerRabbitMQ<PulseIssueReceive, PulseRequestModel, TResponseModel<bool>>()
            .RegisterListenerRabbitMQ<PulseJournalSelectReceive, TAuthRequestStandardModel<TPaginationRequestStandardModel<UserIssueModel>>, TResponseModel<TPaginationResponseStandardModel<PulseViewModel>>>()
            .RegisterListenerRabbitMQ<TelegramMessageIncomingReceive, TelegramIncomingMessageModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<ConsoleIssuesSelectReceive, TPaginationRequestStandardModel<ConsoleIssuesRequestModel>, TPaginationResponseStandardModel<IssueHelpDeskModel>>()
            ;
    }
}