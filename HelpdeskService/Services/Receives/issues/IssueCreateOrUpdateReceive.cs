﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using RemoteCallLib;
using SharedLib;
using DbcLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Create (or update) Issue: Рубрика, тема и описание
/// </summary>
public class IssueCreateOrUpdateReceive(
    IDbContextFactory<HelpdeskContext> helpdeskDbFactory,
    IHelpdeskRemoteTransmissionService helpdeskRemoteRepo,
    ISerializeStorageRemoteTransmissionService SerializeStorageRepo,
    IWebRemoteTransmissionService webTransmissionRepo)
    : IResponseReceive<TAuthRequestModel<IssueUpdateRequestModel>?, int>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.IssueUpdateHelpdeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> ResponseHandleAction(TAuthRequestModel<IssueUpdateRequestModel>? issue_upd)
    {
        ArgumentNullException.ThrowIfNull(issue_upd);
        TResponseModel<int> res = new() { Response = 0 };

        TResponseModel<UserInfoModel[]?> rest = await webTransmissionRepo.FindUsersIdentity([issue_upd.SenderActionUserId]);
        if (!rest.Success() || rest.Response is null || rest.Response.Length != 1)
            return new() { Messages = rest.Messages };

        UserInfoModel actor = rest.Response[0];

        issue_upd.Payload.Description = issue_upd.Payload.Description?.Trim();
        issue_upd.Payload.Name = issue_upd.Payload.Name.Trim();
        string normalizedNameUpper = issue_upd.Payload.Name.ToUpper();
        IssueHelpdeskModelDB issue;
        DateTime dtn = DateTime.UtcNow;
        string msg;
        using HelpdeskContext context = await helpdeskDbFactory.CreateDbContextAsync();


        TResponseModel<ModesSelectRubricsEnum?> res_ModeSelectingRubrics = await SerializeStorageRepo.ReadParameter<ModesSelectRubricsEnum?>(GlobalStaticConstants.CloudStorageMetadata.ModeSelectingRubrics);
        ModesSelectRubricsEnum _current_mode_rubric = res_ModeSelectingRubrics.Response ?? ModesSelectRubricsEnum.AllowWithoutRubric;
        string[] sub_rubrics = await context
            .RubricsForIssues
            .Where(x => x.ParentRubricId == issue_upd.Payload.RubricId)
            .Select(x => x.Name)
            .ToArrayAsync();

        if (sub_rubrics.Length != 0)
        {
            res.AddError($"Требуется выбрать все подрубрики: {string.Join(",", sub_rubrics)};");
            return res;
        }

        if (issue_upd.Payload.Id < 1)
        {
            issue = new()
            {
                AuthorIdentityUserId = issue_upd.SenderActionUserId,
                Name = issue_upd.Payload.Name,
                Description = issue_upd.Payload.Description,
                RubricIssueId = issue_upd.Payload.RubricId,
                NormalizedNameUpper = normalizedNameUpper,
                StepIssue = HelpdeskIssueStepsEnum.Created,
                ProjectId = issue_upd.Payload.ProjectId,
                CreatedAt = dtn,
                LastUpdateAt = dtn,
            };

            IssueReadMarkerHelpdeskModelDB my_mark = new() { Issue = issue, LastReadAt = dtn, UserIdentityId = issue_upd.SenderActionUserId };
            issue.ReadMarkers = [my_mark];

            SubscriberIssueHelpdeskModelDB my_subscr = new() { Issue = issue, UserId = issue_upd.SenderActionUserId };
            issue.Subscribers = [my_subscr];

            await context.AddAsync(issue);
            await context.SaveChangesAsync();
            msg = "Обращение успешно создано";
            res.AddSuccess(msg);
            res.Response = issue.Id;

            await helpdeskRemoteRepo.PulsePush(new()
            {
                SenderActionUserId = issue_upd.SenderActionUserId,
                Payload = new()
                {
                    IssueId = issue.Id,
                    PulseType = PulseIssuesTypesEnum.Status,
                    Tag = issue.StepIssue.DescriptionInfo(),
                    Description = msg,
                }
            });

            await helpdeskRemoteRepo.MessageCreateOrUpdate(new()
            {
                SenderActionUserId = GlobalStaticConstants.Roles.System,
                Payload = new()
                {
                    MessageText = $"Пользователь `{actor.UserName}` создал новый запрос: {issue_upd.Payload.Name}",
                    IssueId = issue.Id
                }
            });
        }
        else
        {
            issue = await context
                .Issues
                .Include(x => x.Subscribers)
                .FirstAsync(x => x.Id == issue_upd.Payload.Id);

            if (issue.AuthorIdentityUserId == issue_upd.SenderActionUserId ||
                issue.ExecutorIdentityUserId == issue_upd.SenderActionUserId ||
                issue.Subscribers?.Any(x => x.UserId == issue_upd.SenderActionUserId) == true ||
                actor.Roles?.Any(x => GlobalStaticConstants.Roles.AllHelpDeskRoles.Contains(x)) == true ||
                actor.IsAdmin)
            {
                res.Response = await context.Issues.Where(x => x.Id == issue_upd.Payload.Id)
                                .ExecuteUpdateAsync(setters => setters
                                .SetProperty(b => b.Description, issue_upd.Payload.Description)
                                .SetProperty(b => b.NormalizedNameUpper, normalizedNameUpper)
                                .SetProperty(b => b.RubricIssueId, issue_upd.Payload.RubricId)
                                .SetProperty(b => b.Name, issue_upd.Payload.Name)
                                .SetProperty(b => b.LastUpdateAt, DateTime.UtcNow));

                msg = "Обращение успешно обновлено";
                res.AddSuccess(msg);
                await helpdeskRemoteRepo.PulsePush(new()
                {
                    SenderActionUserId = issue_upd.SenderActionUserId,
                    Payload = new()
                    {
                        IssueId = issue.Id,
                        PulseType = PulseIssuesTypesEnum.Main,
                        Tag = GlobalStaticConstants.Routes.UPDATE_ACTION_NAME,
                        Description = msg,
                    }
                });

                if (issue_upd.SenderActionUserId != GlobalStaticConstants.Roles.System && issue.Subscribers?.Any(x => x.UserId == issue_upd.SenderActionUserId) != true)
                {
                    await helpdeskRemoteRepo.SubscribeUpdate(new()
                    {
                        SenderActionUserId = GlobalStaticConstants.Roles.System,
                        Payload = new()
                        {
                            IssueId = issue.Id,
                            SetValue = true,
                            UserId = actor.UserId,
                            IsSilent = false,
                        }
                    });
                }
            }
            else
                res.AddError($"У вас не достаточно прав для редактирования этого обращения #{issue_upd.Payload.Id} '{issue.Name}'");
        }

        return res;
    }
}