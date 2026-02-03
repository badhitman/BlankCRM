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
public partial class WebChatService : IWebChatService
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>> SelectUsersJoinsDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel> req, CancellationToken token = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);

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

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        if (req.Payload.IsExclusiveJoin && await context.UsersDialogsJoins.AnyAsync(x => x.DialogJoinId == req.Payload.DialogJoinId && x.OutDateUTC == null, cancellationToken: token))
            return ResponseBaseModel.CreateWarning("Чат уже обслуживается");

        if (await context.UsersDialogsJoins.AnyAsync(x => x.UserIdentityId == req.Payload.UserIdentityId && x.DialogJoinId == req.Payload.DialogJoinId && x.OutDateUTC == null, cancellationToken: token))
            return ResponseBaseModel.CreateSuccess("Пользователь уже участвует в диалоге!");

        TResponseModel<UserInfoModel[]> getUser = await identityRepo.GetUsersOfIdentityAsync([req.Payload.UserIdentityId], token);

        UserJoinDialogWebChatModelDB joinDb = UserJoinDialogWebChatModelDB.Build(req.Payload);
        joinDb.JoinedDateUTC = DateTime.UtcNow;

        await context.UsersDialogsJoins.AddAsync(joinDb, token);
        await context.Messages.AddAsync(new()
        {
            Text = $"К чату присоединялся `{getUser.Response?.FirstOrDefault(x => x.UserId == req.Payload.UserIdentityId)?.UserName ?? req.Payload.UserIdentityId}`",
            CreatedAtUTC = DateTime.UtcNow,
            DialogOwnerId = req.Payload.DialogJoinId,
            SenderUserIdentityId = GlobalStaticConstantsRoles.Roles.System,
        }, token);

        await context.SaveChangesAsync(token);
        //await context.Dialogs
        //    .Where(x => x.Id == joinDb.DialogJoinId)
        //    .ExecuteUpdateAsync(set => set
        //        .SetProperty(p => p.LastMessageAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserJoinDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);

        await context.UsersDialogsJoins
            .Where(x => x.Id == req.Payload && x.OutDateUTC == null)
            .ExecuteUpdateAsync(set => set.SetProperty(p => p.OutDateUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}