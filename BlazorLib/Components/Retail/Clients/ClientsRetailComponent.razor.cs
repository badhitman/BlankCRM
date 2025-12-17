////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor;
using SharedLib;
using System.Net.Mail;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// ClientsRetailComponent
/// </summary>
public partial class ClientsRetailComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavigationRepo { get; set; } = default!;


    List<WalletRetailModelDB> WalletsCache = [];

    string? searchString;
    MudTable<UserInfoModel>? table;
    UserInfoBaseModel newUser = new()
    {
        UserName = ""
    };

    bool DisableCreatingUser
    {
        get
        {
            if (string.IsNullOrWhiteSpace(newUser.PhoneNumber) || string.IsNullOrWhiteSpace(newUser.GivenName) || string.IsNullOrWhiteSpace(newUser.Surname))
                return true;

            if (!string.IsNullOrWhiteSpace(newUser.PhoneNumber))
            {
                newUser.PhoneNumber = newUser.PhoneNumber.Trim();
                if (!GlobalTools.IsPhoneNumber(newUser.PhoneNumber))
                    return true;
            }

            return false;
        }
    }

    async Task CreateNewUser()
    {
        if (CurrentUserSession is null)
            return;

        await SetBusyAsync();
        TResponseModel<string> res = await IdentityRepo.CreateUserManualAsync(new TAuthRequestModel<UserInfoBaseModel>
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = newUser
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);

        if (res.Success())
            NavigationRepo.NavigateTo($"/retail/client-card/{res.Response}");

    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    async Task<TableData<UserInfoModel>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);

        TPaginationResponseModel<UserInfoModel> res = await IdentityRepo.FindUsersAsync(new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize
        }, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        if (res.Response is not null && res.Response.Count != 0)
            await UpdateWallets(res.Response.Select(x => x.UserId), token);

        await SetBusyAsync(false, token);
        return new TableData<UserInfoModel>()
        {
            TotalItems = res.TotalRowsCount,
            Items = res.Response
        };
    }

    async Task UpdateWallets(IEnumerable<string> usersIds, CancellationToken token)
    {
        usersIds = usersIds.Where(x => !WalletsCache.Any(y => y.UserIdentityId == x));
        if (!usersIds.Any())
            return;

        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> reqWallets = new()
        {
            PageNum = 0,
            PageSize = int.MaxValue,
            Payload = new()
            {
                UsersFilterIdentityId = [.. usersIds],
                AutoGenerationWallets = true,
            }
        };
        TPaginationResponseModel<WalletRetailModelDB> res = await RetailRepo.SelectWalletsAsync(reqWallets, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);

        if (res.Response is not null && res.Response.Count != 0)
            WalletsCache.AddRange(res.Response);

        await SetBusyAsync(false, token);
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        await table!.ReloadServerData();
    }
}