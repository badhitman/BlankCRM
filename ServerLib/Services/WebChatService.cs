////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace ServerLib;

/// <summary>
/// WebChatService
/// </summary>
public partial class WebChatService(IDbContextFactory<MainAppContext> mainDbFactory) : IWebChatService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateMessageWebChatAsync(MessageWebChatModelDB req, CancellationToken token = default)
    {
        req.CreatedAtUTC = DateTime.UtcNow;
        req.Text = req.Text.Trim();

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        await context.Messages.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new()
        {
            Response = req.Id
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteToggleMessageWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        if (req.Payload < 1)
            return ResponseBaseModel.CreateError("req.Payload < 1");

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        await context.Messages
            .Where(x => x.Id == req.Payload)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsDisabled, r => !r.IsDisabled), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<SelectMessagesForWebChatResponseModel>> SelectMessagesWebChatAsync(SelectMessagesForWebChatRequestModel req, CancellationToken token = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);

        IQueryable<MessageWebChatModelDB> q = context.Messages
            .Where(x => x.DialogOwner!.SessionTicketId == req.SessionTicketId)
            .Where(x => req.IncludeDeletedMessages || !x.IsDisabled)
            ;

        return new()
        {
            Response = new()
            {
                SessionTicketId = req.SessionTicketId,
                Count = req.Count,
                StartIndex = req.StartIndex,
                IncludeDeletedMessages = req.IncludeDeletedMessages,
                SessionTicket = req.SessionTicket,
                TotalRowsCount = await q.CountAsync(cancellationToken: token),
                Messages = await q
                    .OrderByDescending(x => x.CreatedAtUTC)
                    .Skip(req.StartIndex)
                    .Take(req.Count)
                    .ToListAsync(cancellationToken: token),
            }
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateMessageWebChatAsync(TAuthRequestStandardModel<MessageWebChatModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        await context.Messages
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Text, req.Payload.Text), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<InitWebChatSessionResponseModel>> InitWebChatSessionAsync(InitWebChatSessionRequestModel req, CancellationToken cancellationToken = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        DialogWebChatModelDB? readSession = !string.IsNullOrWhiteSpace(req.SessionTicket)
            ? await context.Dialogs.FirstOrDefaultAsync(x => x.SessionTicketId == req.SessionTicket && !x.IsDisabled && x.DeadlineUTC >= DateTime.UtcNow, cancellationToken: cancellationToken)
            : new()
            {
                SessionTicketId = $"{Guid.NewGuid()}/{Guid.NewGuid()}",
                DeadlineUTC = DateTime.UtcNow.AddSeconds(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds),
                CreatedAtUTC = DateTime.UtcNow,
                LastReadAtUTC = DateTime.UtcNow,
            };

        readSession ??= new()
        {
            SessionTicketId = $"{Guid.NewGuid()}/{Guid.NewGuid()}",
            DeadlineUTC = DateTime.UtcNow.AddSeconds(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds),
            CreatedAtUTC = DateTime.UtcNow,
            LastReadAtUTC = DateTime.UtcNow,
            InitiatorIdentityId = req.UserIdentityId,
        };

        if (readSession.Id == 0)
        {
            await context.Dialogs.AddAsync(readSession, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await context.Dialogs.Where(x => x.Id == readSession.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.DeadlineUTC, DateTime.UtcNow.AddMinutes(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds)), cancellationToken: cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(req.UserIdentityId) && string.IsNullOrWhiteSpace(readSession.InitiatorIdentityId))
            await context.Dialogs.Where(x => x.Id == readSession.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.InitiatorIdentityId, req.UserIdentityId), cancellationToken: cancellationToken);

        return new()
        {
            Response = new()
            {
                SessionTicket = readSession.SessionTicketId,
                DialogId = readSession.Id,
                DeadlineUTC = readSession.DeadlineUTC
            }
        };
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}