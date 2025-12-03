////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Net.Mail;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// ClientsRetailComponent
/// </summary>
public partial class ClientsRetailComponent : BlazorBusyComponentBaseAuthModel
{
    //[Inject]
    //IRetailService retailRepo { get; set; } = default!;
    [Inject]
    NavigationManager NavigationRepo { get; set; } = default!;


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
            if (string.IsNullOrWhiteSpace(newUser.UserName))
                return true;

            if (!MailAddress.TryCreate(newUser.UserName, out _))
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

        await SetBusyAsync(false, token);
        return new TableData<UserInfoModel>()
        {
            TotalItems = res.TotalRowsCount,
            Items = res.Response
        };
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        await table!.ReloadServerData();
    }
}