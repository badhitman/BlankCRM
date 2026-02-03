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
    public async Task<TPaginationResponseStandardModel<DialogWebChatViewModel>> SelectDialogsWebChatsAsync(TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel> req, CancellationToken token = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);

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
            Response = await q.Skip(req.PageNum * req.PageSize).Take(req.PageSize).Select(x => new DialogWebChatViewModel()
            {
                SessionTicketId = x.SessionTicketId,
                CreatedAtUTC = x.CreatedAtUTC,
                DeadlineUTC = x.DeadlineUTC,
                Id = x.Id,
                InitiatorContacts = x.InitiatorContacts,
                InitiatorHumanName = x.InitiatorHumanName,
                InitiatorIdentityId = x.InitiatorIdentityId,
                IsDisabled = x.IsDisabled,
                LastMessageAtUTC = x.LastMessageAtUTC,
                LastReadAtUTC = x.LastReadAtUTC,
            }).ToListAsync(cancellationToken: token),
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDialogWebChatAsync(TAuthRequestStandardModel<DialogWebChatBaseModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return ResponseBaseModel.CreateError("req.Payload is null");

        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<DialogWebChatModelDB> q = context.Dialogs.Where(x => x.Id == req.Payload.Id);
        await q.ExecuteUpdateAsync(set => set
            .SetProperty(p => p.InitiatorContacts, req.Payload.InitiatorContacts)
            .SetProperty(p => p.InitiatorHumanName, req.Payload.InitiatorHumanName)
            .SetProperty(p => p.InitiatorContactsNormalized, req.Payload.InitiatorHumanName?.ToUpper())
            .SetProperty(p => p.InitiatorIdentityId, req.Payload.InitiatorIdentityId), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteToggleDialogWebChatAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
    {
        MainAppContext context = await mainDbFactory.CreateDbContextAsync(token);
        IQueryable<DialogWebChatModelDB> q = context.Dialogs.Where(x => x.Id == req.Payload).AsQueryable();

        await q.ExecuteUpdateAsync(set => set
           .SetProperty(p => p.IsDisabled, r => !r.IsDisabled), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}