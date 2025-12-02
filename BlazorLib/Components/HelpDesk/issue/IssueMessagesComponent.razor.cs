////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// IssueMessagesComponent
/// </summary>
public partial class IssueMessagesComponent : IssueWrapBaseModel
{
    MudTable<IssueMessageHelpDeskModelDB>? tableRef;

    /// <summary>
    /// Добавляется новое сообщение
    /// </summary>
    public bool AddingNewMessage = false;

    private string _searchStringQuery = "";
    private string searchStringQuery
    {
        get => _searchStringQuery;
        set
        {
            _searchStringQuery = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }

    IssueMessageHelpDeskModelDB[]? messages;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private Task<TableData<IssueMessageHelpDeskModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (messages is null)
            return Task.FromResult(new TableData<IssueMessageHelpDeskModelDB>() { TotalItems = 0, Items = [] });

        IssueMessageHelpDeskModelDB[] _messages = string.IsNullOrWhiteSpace(searchStringQuery)
            ? messages
            : [.. messages.Where(x => x.MessageText.Contains(searchStringQuery, StringComparison.OrdinalIgnoreCase))];

        return Task.FromResult(new TableData<IssueMessageHelpDeskModelDB>() { TotalItems = _messages.Length, Items = _messages.OrderBy(x => x.Id).Skip(state.PageSize * state.Page).Take(state.PageSize) });
    }

    /// <summary>
    /// ReloadMessages
    /// </summary>
    public async Task ReloadMessages()
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        TResponseModel<IssueMessageHelpDeskModelDB[]> messages_rest = await HelpDeskRepo.MessagesListAsync(new()
        {
            Payload = Issue.Id,
            SenderActionUserId = CurrentUserSession.UserId,
        });
        IsBusyProgress = false;
        messages = messages_rest.Response;
        SnackBarRepo.ShowMessagesResponse(messages_rest.Messages);
        if (!messages_rest.Success() || messages_rest.Response is null)
            return;

        Issue.Messages = [.. messages_rest.Response];

        string[] users_for_adding = Issue
            .Messages
            .Where(x => x.AuthorUserId != GlobalStaticConstantsRoles.Roles.System && !UsersIdentityDump.Any(y => y.UserId == x.AuthorUserId))
            .Select(x => x.AuthorUserId)
            .ToArray();

        if (users_for_adding.Length != 0)
        {
            TResponseModel<UserInfoModel[]> users_data_identity = await IdentityRepo.GetUsersOfIdentityAsync([.. users_for_adding.Distinct()]);
            SnackBarRepo.ShowMessagesResponse(users_data_identity.Messages);
            if (users_data_identity.Response is not null && users_data_identity.Response.Length != 0)
                UsersIdentityDump.AddRange(users_data_identity.Response);
        }
        if (tableRef is not null)
            await tableRef.ReloadServerData();
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadMessages();
    }
}