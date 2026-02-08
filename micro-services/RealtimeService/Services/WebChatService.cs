////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DbcLib;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Server;
using SharedLib;

namespace RealtimeService;

/// <summary>
/// WebChatService
/// </summary>
public partial class WebChatService(IDbContextFactory<RealtimeContext> mainDbFactory, IIdentityTransmission identityRepo, IEventsWebChatsNotifies notifyWebChatRepo, MqttServer mqttServerRepo)
    : IWebChatService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<MqttClientModel>>> GetClientsConnectionsAsync(GetClientsRequestModel req, CancellationToken cancellationToken = default)
    {
        IList<MqttSessionStatus> v = await mqttServerRepo.GetSessionsAsync();
        
        return new()
        {
            Response = [..(await mqttServerRepo.GetClientsAsync()).Select(x=> new MqttClientModel()
            {
                Id = x.Id,
                BytesSent = x.BytesSent,
                BytesReceived = x.BytesReceived,
                ConnectedTimestamp = x.ConnectedTimestamp,
                LastNonKeepAlivePacketReceivedTimestamp = x.LastNonKeepAlivePacketReceivedTimestamp,
                LastPacketReceivedTimestamp = x.LastPacketReceivedTimestamp,
                LastPacketSentTimestamp = x.LastPacketSentTimestamp,
                ProtocolVersion = x.ProtocolVersion.ToString(),
                ReceivedApplicationMessagesCount = x.ReceivedApplicationMessagesCount,
                ReceivedPacketsCount = x.ReceivedPacketsCount,
                RemoteEndPoint = x.RemoteEndPoint.ToString(),
                SentApplicationMessagesCount = x.SentApplicationMessagesCount,
                SentPacketsCount = x.SentPacketsCount,
            })]
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<DialogWebChatModelDB>> InitWebChatSessionAsync(InitWebChatSessionRequestModel req, CancellationToken cancellationToken = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(cancellationToken);
        DialogWebChatModelDB? readSession = !string.IsNullOrWhiteSpace(req.SessionTicket)
            ? await context.Dialogs.Include(x => x.UsersJoins).FirstOrDefaultAsync(x => x.SessionTicketId == req.SessionTicket && !x.IsDisabled && x.DeadlineUTC >= DateTime.UtcNow, cancellationToken: cancellationToken)
            : new()
            {
                SessionTicketId = $"{Guid.NewGuid()}/{Guid.NewGuid()}",
                DeadlineUTC = DateTime.UtcNow.AddSeconds(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds),
                CreatedAtUTC = DateTime.UtcNow,
                LastOnlineAtUTC = DateTime.UtcNow,
                InitiatorIdentityId = req.UserIdentityId,
                Language = req.Language,
                UserAgent = req.UserAgent,
            };

        readSession ??= new()
        {
            SessionTicketId = $"{Guid.NewGuid()}/{Guid.NewGuid()}",
            DeadlineUTC = DateTime.UtcNow.AddSeconds(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds),
            CreatedAtUTC = DateTime.UtcNow,
            LastOnlineAtUTC = DateTime.UtcNow,
            InitiatorIdentityId = req.UserIdentityId,
            Language = req.Language,
            UserAgent = req.UserAgent,
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
                    .SetProperty(p => p.LastOnlineAtUTC, DateTime.UtcNow)
                    .SetProperty(p => p.DeadlineUTC, DateTime.UtcNow.AddMinutes(GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds)), cancellationToken: cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(req.UserIdentityId) && string.IsNullOrWhiteSpace(readSession.InitiatorIdentityId))
            await context.Dialogs.Where(x => x.Id == readSession.Id)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.InitiatorIdentityId, req.UserIdentityId), cancellationToken: cancellationToken);

        return new()
        {
            Response = readSession
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<DialogWebChatModelDB>>> DialogsWebChatsReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.Dialogs
                .Where(x => req.Payload.Contains(x.Id))
                .Include(x => x.UsersJoins)
                .ToListAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DialogWebChatModelDB>> SelectDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel> req, CancellationToken token = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);

        if (req.PageSize < 10)
            req.PageSize = 10;

        IQueryable<DialogWebChatModelDB> q = context.Dialogs.AsQueryable();
        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.InitiatorContactsNormalized != null && x.InitiatorContactsNormalized.Contains(req.FindQuery.ToUpper()));

        if (req.Payload?.IsDisabledFiltered is not null)
            q = q.Where(x => x.IsDisabled == req.Payload.IsDisabledFiltered);

        if (!string.IsNullOrWhiteSpace(req.Payload?.FilterUserIdentityId))
            q = q.Where(x => x.InitiatorIdentityId == req.Payload.FilterUserIdentityId);

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q
                             .OrderByDescending(x => x.LastMessageAtUTC)
                             .Skip(req.PageNum * req.PageSize)
                             .Take(req.PageSize)
                             .Include(x => x.UsersJoins)
                             .ToListAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDialogWebChatInitiatorAsync(TAuthRequestStandardModel<DialogWebChatBaseModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<DialogWebChatModelDB> q = context.Dialogs.Where(x => x.Id == req.Payload.Id);
        await q.ExecuteUpdateAsync(set => set
            .SetProperty(p => p.InitiatorContacts, req.Payload.InitiatorContacts)
            .SetProperty(p => p.InitiatorHumanName, req.Payload.InitiatorHumanName)
            .SetProperty(p => p.InitiatorContactsNormalized, req.Payload.InitiatorHumanName?.ToUpper()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDialogWebChatAdminAsync(TAuthRequestStandardModel<DialogWebChatBaseModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
            return ResponseBaseModel.CreateError("string.IsNullOrWhiteSpace(req.SenderActionUserId)");

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<DialogWebChatModelDB> q = context.Dialogs.Where(x => x.Id == req.Payload.Id);
        await q.ExecuteUpdateAsync(set => set
            .SetProperty(p => p.InitiatorContacts, req.Payload.InitiatorContacts)
            .SetProperty(p => p.InitiatorHumanName, req.Payload.InitiatorHumanName)
            .SetProperty(p => p.InitiatorIdentityId, req.Payload.InitiatorIdentityId)
            .SetProperty(p => p.InitiatorContactsNormalized, req.Payload.InitiatorHumanName?.ToUpper()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteToggleDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<DialogWebChatModelDB> q = context.Dialogs.Where(x => x.Id == req.Payload).AsQueryable();

        await q.ExecuteUpdateAsync(set => set
           .SetProperty(p => p.IsDisabled, r => !r.IsDisabled), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}