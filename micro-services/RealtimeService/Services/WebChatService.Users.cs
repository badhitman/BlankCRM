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
    public async Task<TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>> SelectUsersJoinsDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel> req, CancellationToken token = default)
    {
        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);

        if (req.PageSize < 10)
            req.PageSize = 10;

        IQueryable<UserJoinDialogWebChatModelDB> q = context.UsersDialogsJoins.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.Payload?.FilterUserIdentityId))
            q = q.Where(x => x.UserIdentityId == req.Payload.FilterUserIdentityId);

        return new()
        {
            PageSize = req.PageSize,
            PageNum = req.PageNum,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await q.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UserInjectDialogWebChatAsync(TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        if (req.Payload.IsExclusiveJoin && await context.UsersDialogsJoins.AnyAsync(x => x.DialogJoinId == req.Payload.DialogJoinId && x.OutDateUTC == null, cancellationToken: token))
            return ResponseBaseModel.CreateWarning("Чат уже обслуживается");

        if (await context.UsersDialogsJoins.AnyAsync(x => x.UserIdentityId == req.Payload.UserIdentityId && x.DialogJoinId == req.Payload.DialogJoinId && x.OutDateUTC == null, cancellationToken: token))
            return ResponseBaseModel.CreateSuccess("Пользователь уже участвует в диалоге!");

        UserJoinDialogWebChatModelDB joinDb = UserJoinDialogWebChatModelDB.Build(req.Payload);
        joinDb.JoinedDateUTC = DateTime.UtcNow;

        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        await context.UsersDialogsJoins.AddAsync(joinDb, token);
        await context.SaveChangesAsync(token);

        TResponseModel<UserInfoModel[]> getUser = await identityRepo.GetUsersOfIdentityAsync([req.Payload.UserIdentityId], token);
        UserInfoModel? userData = getUser.Response?.FirstOrDefault(x => x.UserId == req.Payload.UserIdentityId);
        string textMsg = $"К чату присоединился `{userData?.UserName ?? req.Payload.UserIdentityId}`";

        MessageWebChatModelDB newMsgDb = new()
        {
            Text = textMsg,
            CreatedAtUTC = DateTime.UtcNow,
            DialogOwnerId = req.Payload.DialogJoinId,
            SenderUserIdentityId = GlobalStaticConstantsRoles.Roles.System,
        };
        TResponseModel<int> resAddingMsg = await CreateMessageWebChatAsync(newMsgDb, token);
        if (!resAddingMsg.Success() || resAddingMsg.Response < 1)
        {
            await transaction.RollbackAsync(token);
            return new() { Messages = resAddingMsg.Messages };
        }

        IQueryable<DialogWebChatModelDB> q = context.Dialogs
            .Where(x => x.Id == req.Payload.DialogJoinId);
        await q.ExecuteUpdateAsync(set => set
                .SetProperty(p => p.LastMessageAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserJoinDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req.SenderActionUserId))
            return ResponseBaseModel.CreateError($"string.IsNullOrWhiteSpace(req.SenderActionUserId) > {nameof(DeleteUserJoinDialogWebChatAsync)}");

        RealtimeContext context = await mainDbFactory.CreateDbContextAsync(token);
        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        IQueryable<UserJoinDialogWebChatModelDB> q = context.UsersDialogsJoins
            .Where(x => x.Id == req.Payload);
        await q
            .Where(x => x.OutDateUTC == null)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.OutDateUTC, DateTime.UtcNow), cancellationToken: token);

        TResponseModel<UserInfoModel[]> getUser = await identityRepo.GetUsersOfIdentityAsync([req.SenderActionUserId], token);
        UserInfoModel? userData = getUser.Response?.FirstOrDefault(x => x.UserId == req.SenderActionUserId);
        int dialogId = await q.Select(x => x.DialogJoinId).FirstAsync(cancellationToken: token);
        string textMsg = $"Из чата вышел `{getUser.Response?.FirstOrDefault(x => x.UserId == req.SenderActionUserId)?.UserName ?? req.SenderActionUserId}`";

        MessageWebChatModelDB newMsgDb = new()
        {
            Text = textMsg,
            CreatedAtUTC = DateTime.UtcNow,
            DialogOwnerId = dialogId,
            SenderUserIdentityId = GlobalStaticConstantsRoles.Roles.System,
        };
        TResponseModel<int> resAddingMsg = await CreateMessageWebChatAsync(newMsgDb, token);
        if (!resAddingMsg.Success() || resAddingMsg.Response < 1)
        {
            await transaction.RollbackAsync(token);
            return new() { Messages = resAddingMsg.Messages };
        }

        await context.Dialogs
            .Where(x => x.Id == req.Payload)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.LastMessageAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);

        if (userData?.TelegramId.HasValue == true)
        {

            SendTextMessageTelegramBotModel tgMsgSend = new()
            {
                From = "Уведомление",
                Message = $"Вы покинули чат: {await q.Select(x => x.DialogJoin!.BaseUri).FirstAsync(cancellationToken: token)}web-chats/room-{req.Payload}",
                UserTelegramId = userData.TelegramId.Value,
            };
            await tgRepo.SendTextMessageTelegramAsync(tgMsgSend, waitResponse: false, token: token);
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}