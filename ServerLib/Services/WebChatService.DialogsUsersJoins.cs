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
    public async Task<ResponseBaseModel> CreateUserJoinDialogWebChatAsync(TAuthRequestStandardModel<UserJoinDialogWebChatModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);

        await context.AddAsync(req.Payload, token);
        await context.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteUserJoinDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);

        await context.UsersDialogsJoins
            .Where(x => x.Id == req.Payload)
            .ExecuteDeleteAsync(cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}