////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Helpdesk;

/// <summary>
/// HelpdeskJournalComponent
/// </summary>
public partial class HelpdeskJournalComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IHelpdeskTransmission HelpdeskRepo { get; set; } = default!;


    /// <summary>
    ///Journal mode
    /// </summary>
    [Parameter, EditorRequired]
    public required HelpdeskJournalModesEnum JournalMode { get; set; }

    /// <summary>
    /// UserArea
    /// </summary>
    [Parameter, EditorRequired]
    public required UsersAreasHelpdeskEnum? UserArea { get; set; }

    /// <summary>
    /// UserIdentityId
    /// </summary>
    [Parameter]
    public string? UserIdentityId { get; set; }

    /// <summary>
    /// SetTab
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required Action<HelpdeskJournalComponent> SetTab { get; set; }


    /// <summary>
    /// SetArea
    /// </summary>
    public void SetArea(UsersAreasHelpdeskEnum? set)
    {
        UserArea = set;
    }

    private string? searchString = null;

    readonly List<UserInfoModel> usersDump = [];

    /// <inheritdoc/>
    public MudTable<IssueHelpdeskModel> TableRef = default!;
    bool doneLoadTable;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        SetTab(this);
        if (string.IsNullOrWhiteSpace(UserIdentityId))
            UserIdentityId = CurrentUserSession.UserId;

        await SetBusyAsync(false);
        doneLoadTable = true;
        if (TableRef is not null)
            await TableRef.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    private async Task<TableData<IssueHelpdeskModel>> ServerReload(TableState state, CancellationToken token)
    {
        if (!doneLoadTable || CurrentUserSession is null)
            return new() { TotalItems = 0, Items = [] };

        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectIssuesRequestModel> req = new()
        {
            Payload = new()
            {
                IdentityUsersIds = [UserIdentityId!],
                JournalMode = JournalMode,
                SearchQuery = searchString,
                UserArea = UserArea,
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };

        TResponseModel<TPaginationResponseStandardModel<IssueHelpdeskModel>> rest = await HelpdeskRepo
             .IssuesSelectAsync(new() { Payload = req, SenderActionUserId = CurrentUserSession.UserId }, token);

        await SetBusyAsync(false, token);
        if (rest.Response?.Response is null)
            return new() { TotalItems = 0, Items = [] }; ;

        // Forward the provided token to methods which support it
        List<IssueHelpdeskModel> data = rest.Response.Response;
        await UpdateUsersData([.. data.SelectMany(x => new string?[] { x.AuthorIdentityUserId, x.ExecutorIdentityUserId })]);
        // Return the data
        return new() { TotalItems = rest.Response.TotalRowsCount, Items = data };
    }

    async Task UpdateUsersData(string?[] users_ids)
    {
        string[] _ids = [.. users_ids.Where(x => !string.IsNullOrWhiteSpace(x) && !usersDump.Any(y => y.UserId == x))!];
        if (_ids.Length == 0)
            return;

        await SetBusyAsync();

        TResponseModel<UserInfoModel[]> res = await IdentityRepo.GetUsersOfIdentityAsync(_ids);
        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is null)
            return;
        usersDump.AddRange(res.Response);
    }

    private void OnSearch(string text)
    {
        searchString = text;
        TableRef.ReloadServerData();
    }
}