////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

/// <summary>
/// TelegramChatsTableComponent
/// </summary>
public partial class TelegramChatsTableComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ITelegramTransmission TgRepo { get; set; } = default!;

    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;


    private IEnumerable<ChatTelegramStandardModel> pagedData = [];
    private MudTable<ChatTelegramStandardModel> table = default!;

    private string? searchString = null;

    readonly List<UserInfoModel> UsersCache = [];
    readonly Dictionary<string, IssueHelpDeskModel[]> IssuesCache = [];

    async Task<TableData<ChatTelegramStandardModel>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<string?> req = new()
        {
            Payload = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        TPaginationResponseStandardModel<ChatTelegramStandardModel> rest = await TgRepo.ChatsSelectTelegramAsync(req, token);

        if (rest.Response is null)
            return new TableData<ChatTelegramStandardModel>() { TotalItems = 0, Items = pagedData };
        pagedData = rest.Response;
        await LoadUsersData();
        await SetBusyAsync(false, token);
        return new TableData<ChatTelegramStandardModel>() { TotalItems = rest.TotalRowsCount, Items = pagedData };
    }

    async Task LoadUsersData()
    {
        if (CurrentUserSession is null)
            return;

        IQueryable<ChatTelegramStandardModel> q = pagedData
            .Where(x => x.Type == ChatsTypesTelegramEnum.Private && !UsersCache.Any(y => y.TelegramId == x.ChatTelegramId))
            .AsQueryable();

        long[] users_ids_for_load = [.. q.Select(x => x.ChatTelegramId)];
        if (users_ids_for_load.Length == 0)
            return;

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> users_res = await IdentityRepo.GetUsersIdentityByTelegramAsync(users_ids_for_load);

        SnackBarRepo.ShowMessagesResponse(users_res.Messages);
        if (!users_res.Success() || users_res.Response is null || users_res.Response.Length == 0)
        {
            await SetBusyAsync(false);
            return;
        }

        UsersCache.AddRange(users_res.Response);

        string[] users_ids_identity = [.. users_res.Response.Select(x => x.UserId)];
        TAuthRequestStandardModel<TPaginationRequestStandardModel<SelectIssuesRequestModel>> req = new()
        {
            Payload = new()
            {
                Payload = new()
                {
                    IdentityUsersIds = [.. users_ids_identity],
                    JournalMode = HelpDeskJournalModesEnum.All,
                    IncludeSubscribers = true,
                },
                PageNum = 0,
                PageSize = 100,
                SortBy = nameof(IssueHelpDeskModel.LastUpdateAt),
                SortingDirection = DirectionsEnum.Down,
            },
            SenderActionUserId = CurrentUserSession.UserId
        };
        await SetBusyAsync();
        TResponseModel<TPaginationResponseStandardModel<IssueHelpDeskModel>> issues_users_res = await HelpDeskRepo
                     .IssuesSelectAsync(req);

        if (issues_users_res.Response?.Response is null || issues_users_res.Response.Response.Count == 0)
        {
            await SetBusyAsync(false);
            return;
        }

        if (issues_users_res.Response.TotalRowsCount > req.Payload.PageSize)
            SnackBarRepo.Error($"Записей больше: {issues_users_res.Response.TotalRowsCount}");

        foreach (UserInfoModel us in users_res.Response)
        {
            IssueHelpDeskModel[] issues_for_user = [.. issues_users_res.Response.Response
                .Where(x =>
                x.ExecutorIdentityUserId == us.UserId ||
                x.Subscribers!.Any(y => y.UserId == us.UserId) ||
                x.AuthorIdentityUserId == us.UserId)];

            if (issues_for_user.Length != 0)
                IssuesCache.Add(us.UserId, issues_for_user);
        }

        await SetBusyAsync(false);
    }

    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
}