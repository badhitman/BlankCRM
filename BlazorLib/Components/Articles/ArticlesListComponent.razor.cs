////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Articles;

/// <summary>
/// ArticlesListComponent
/// </summary>
public partial class ArticlesListComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;


    MudTable<ArticleModelDB> table = default!;
    bool readyTableLoad;

    string? searchString = null;
    readonly List<UserInfoModel> usersDump = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        readyTableLoad = true;
        if (table is not null)
            await table.ReloadServerData();
        await SetBusyAsync(false);
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    async Task<TableData<ArticleModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (!readyTableLoad || CurrentUserSession is null)
            return new() { TotalItems = 0, Items = [] };

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
        await UpdateUsersData([.. rest.Response!.Select(x => x.AuthorIdentityId)]);
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

    void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
}