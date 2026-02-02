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
public class WebChatService(IDbContextFactory<MainAppContext> mainDbFactory) : IWebChatService
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
    public async Task<ResponseBaseModel> DeleteToggleMessageWebChatAsync(TAuthRequestStandardModel<DeleteToggleMessageWebChatRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        await context.Messages
            .Where(x => x.Id == req.Payload.MessageWebChat)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsDisabled, r => !r.IsDisabled), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }


    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<MessageWebChatModelDB>> SelectMessagesWebChatAsync(TPaginationRequestStandardModel<SelectMessagesForWebChatRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Status = new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] } };

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        if (req.PageSize < 10)
            req.PageSize = 10;

        IQueryable<MessageWebChatModelDB> q = context.Messages
            .Where(x => x.DialogOwner!.SessionTicketId == req.Payload.SessionTicketId)
            .Where(x => req.Payload.IncludeDeletedMessages || !x.IsDisabled)
            ;

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q
                .OrderByDescending(x => x.CreatedAtUTC)
                .Skip(req.PageNum * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync(cancellationToken: token),
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
        };

        if (readSession.Id == 0)
        {
            await context.Dialogs.AddAsync(readSession, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        else
            await context.Dialogs.Where(x => x.Id == readSession.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.DeadlineUTC, DateTime.UtcNow.AddMinutes(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds)), cancellationToken: cancellationToken);

        return new()
        {
            Response = new()
            {
                SessionTicket = readSession.SessionTicketId,
                DialogId = readSession.Id,
            }
        };
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
