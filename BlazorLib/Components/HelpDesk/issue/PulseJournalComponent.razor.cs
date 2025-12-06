////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.HelpDesk.issue;

/// <summary>
/// PulseJournalComponent
/// </summary>
public partial class PulseJournalComponent : IssueWrapBaseModel
{
    static MarkupString ms(string raw_html) => (MarkupString)raw_html;
    MudTable<PulseViewModel>? tableRef;
    bool doneTableLoad;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<PulseViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        if (!doneTableLoad || CurrentUserSession is null)
            return new TableData<PulseViewModel>() { TotalItems = 0, Items = [] };

        await SetBusyAsync(token: token);
        TResponseModel<TPaginationResponseModel<PulseViewModel>> tp = await HelpDeskRepo.PulseSelectJournalAsync(new()
        {
            Payload = new TPaginationRequestStandardModel<UserIssueModel>()
            {
                PageNum = state.Page,
                PageSize = state.PageSize,
                SortingDirection = state.SortDirection == SortDirection.Descending ? DirectionsEnum.Down : DirectionsEnum.Up,
                SortBy = state.SortLabel,
                Payload = new()
                {
                    UserId = CurrentUserSession.UserId,
                    IssueId = Issue.Id,
                }
            },
            SenderActionUserId = CurrentUserSession.UserId,
        }, token);
        SnackBarRepo.ShowMessagesResponse(tp.Messages);

        if (!tp.Success() || tp.Response?.Response is null)
        {
            await SetBusyAsync(false, token);
            return new TableData<PulseViewModel>() { TotalItems = 0, Items = [] };
        }

        string[] users_ids = [.. tp.Response.Response
            .Select(x => x.AuthorUserIdentityId)
            .Where(x => !UsersIdentityDump.Any(y => y.UserId == x))];

        if (users_ids.Length != 0)
        {
            TResponseModel<UserInfoModel[]> users_add = await IdentityRepo.GetUsersOfIdentityAsync(users_ids, token);

            SnackBarRepo.ShowMessagesResponse(users_add.Messages);
            if (users_add.Response is not null)
                UsersIdentityDump.AddRange(users_add.Response);
        }

        await SetBusyAsync(false, token);
        return new TableData<PulseViewModel>() { TotalItems = tp.Response.TotalRowsCount, Items = tp.Response.Response };
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        doneTableLoad = true;
        if (tableRef is not null)
            await tableRef.ReloadServerData();
    }
}