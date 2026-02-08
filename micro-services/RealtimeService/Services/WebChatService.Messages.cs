////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace RealtimeService;

/// <summary>
/// WebChatService
/// </summary>
public partial class WebChatService : IWebChatService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateMessageWebChatAsync(MessageWebChatModelDB req, CancellationToken token = default)
    {
        req.CreatedAtUTC = DateTime.UtcNow;
        req.Text = req.Text.Trim();

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        await context.Messages.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        await context.Dialogs
            .Where(x => x.Id == req.DialogOwnerId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.LastMessageAtUTC, DateTime.UtcNow), cancellationToken: token);

        await notifyWebChatRepo.NewMessageWebChatAsync(new() { DialogId = req.DialogOwnerId }, token);

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

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<MessageWebChatModelDB> q = context.Messages
            .Where(x => x.Id == req.Payload);

        await q.ExecuteUpdateAsync(set => set
                .SetProperty(p => p.IsDisabled, r => !r.IsDisabled), cancellationToken: token);

        await notifyWebChatRepo.NewMessageWebChatAsync(new()
        {
            DialogId = await q.Select(x => x.DialogOwnerId).FirstAsync(cancellationToken: token)
        }, token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<SelectMessagesForWebChatResponseModel>> SelectMessagesWebChatAsync(SelectMessagesForWebChatRequestModel req, CancellationToken token = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);

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
                    .Include(x => x.AttachesFiles)
                    .ToListAsync(cancellationToken: token),
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<MessageWebChatModelDB>> SelectMessagesForRoomWebChatAsync(TPaginationRequestAuthModel<SelectMessagesForWebChatRoomRequestModel> req, CancellationToken token = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);

        IQueryable<MessageWebChatModelDB> q = context.Messages
            .Where(x => x.DialogOwnerId == req.Payload.DialogId)
            .Where(x => req.Payload.IncludeDeletedMessages == true || !x.IsDisabled)
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
                    .Include(x => x.AttachesFiles)
                    .ToListAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateMessageWebChatAsync(TAuthRequestStandardModel<MessageWebChatModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);

        MessageWebChatModelDB msgDb = await context.Messages
            .Where(x => x.Id == req.Payload.Id)
            .Include(x => x.AttachesFiles)
            .FirstAsync(cancellationToken: token);

        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        IQueryable<MessageWebChatModelDB> q = context.Messages.Where(x => x.Id == req.Payload.Id);
        await q.ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Text, req.Payload.Text), cancellationToken: token);

        if (req.Payload.AttachesFiles is not null)
        {
            List<AttachesMessageWebChatModelDB> newFiles = [.. req.Payload.AttachesFiles.Where(x => msgDb.AttachesFiles?.Any(y => y.FileAttachId == x.FileAttachId) != true)];
            if (newFiles.Count != 0)
            {
                newFiles.ForEach(x => x.MessageOwnerId = msgDb.Id);
                await context.AttachesFilesOfMessages.AddRangeAsync(newFiles, token);
                await context.SaveChangesAsync(token);
            }
        }
        await transaction.CommitAsync(token);

        await notifyWebChatRepo.NewMessageWebChatAsync(new()
        {
            DialogId = await q.Select(x => x.DialogOwnerId).FirstAsync(cancellationToken: token)
        }, token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}