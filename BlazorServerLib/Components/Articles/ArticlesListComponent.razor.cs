////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Articles;

/// <summary>
/// ArticlesListComponent
/// </summary>
public partial class ArticlesListComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;


    private MudTable<ArticleModelDB> table = default!;

    private string? searchString = null;
    readonly List<UserInfoModel> usersDump = [];

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<ArticleModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectArticlesRequestModel> req = new()
        {
            Payload = new()
            {
                IdentityUsersIds = [CurrentUserSession.UserId],
                SearchQuery = searchString,
                IncludeExternal = true,
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };

        TPaginationResponseModel<ArticleModelDB> rest = await HelpDeskRepo
            .ArticlesSelectAsync(req, token);

        await SetBusyAsync(false, token: token);

        // Forward the provided token to methods which support it
        List<ArticleModelDB> data = rest.Response!;
        await UpdateUsersData(rest.Response!.Select(x => x.AuthorIdentityId).ToArray());
        // Return the data
        return new() { TotalItems = rest.TotalRowsCount, Items = data };
    }

    async Task UpdateUsersData(string?[] users_ids)
    {
        string[] _ids = [.. users_ids.Where(x => !string.IsNullOrWhiteSpace(x) && !usersDump.Any(y => y.UserId == x))!];
        if (_ids.Length == 0)
            return;

        await SetBusyAsync();

        TResponseModel<UserInfoModel[]> res = await IdentityRepo.GetUsersOfIdentityAsync(_ids);
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is null)
            return;
        usersDump.AddRange(res.Response);
    }

    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
}