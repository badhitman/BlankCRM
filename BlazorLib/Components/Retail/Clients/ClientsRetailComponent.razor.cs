////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// ClientsRetailComponent
/// </summary>
public partial class ClientsRetailComponent : BlazorBusyComponentBaseAuthModel
{
    //[Inject]
    //IRetailService retailRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission identityRepo { get; set; } = default!;


    string? searchString;
    MudTable<UserInfoModel>? table;

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server, with a token for canceling this request
    /// </summary>
    async Task<TableData<UserInfoModel>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationResponseModel<UserInfoModel> res = await identityRepo.FindUsersAsync(new() { FindQuery = searchString, PageNum = state.Page, PageSize = state.PageSize }, token);
        await SetBusyAsync(false, token);
        return new TableData<UserInfoModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async Task OnSearch(string text)
    {
        searchString = text;
        await table!.ReloadServerData();
    }
}