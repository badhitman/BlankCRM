﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Helpdesk (service)
/// </summary>
public interface IHelpdeskService
{
    /// <summary>
    /// ConsoleIssuesSelect
    /// </summary>
    public Task<TPaginationResponseModel<IssueHelpdeskModel>> ConsoleIssuesSelect(TPaginationRequestModel<ConsoleIssuesRequestModel> req);

    /// <summary>
    /// Subscribe update - of context user
    /// </summary>
    public Task<TResponseModel<bool>> ExecuterUpdate(TAuthRequestModel<UserIssueModel> req);

    /// <summary>
    /// Create (or update) Issue: Рубрика, тема и описание
    /// </summary>
    public Task<TResponseModel<int>> IssueCreateOrUpdate(TAuthRequestModel<IssueUpdateRequestModel> req);

    /// <summary>
    /// Регистрация события из обращения (логи).
    /// </summary>
    /// <remarks>
    /// Плюс рассылка уведомлений участникам события.
    /// </remarks>
    public Task<TResponseModel<bool>> PulsePush(PulseRequestModel req);

    /// <summary>
    /// Сообщение в обращение
    /// </summary>
    public Task<TResponseModel<int?>> MessageUpdateOrCreate(TAuthRequestModel<IssueMessageHelpdeskBaseModel> req);

    /// <summary>
    /// Subscribe update - of context user
    /// </summary>
    public Task<TResponseModel<bool?>> SubscribeUpdate(TAuthRequestModel<SubscribeUpdateRequestModel> req);
}
