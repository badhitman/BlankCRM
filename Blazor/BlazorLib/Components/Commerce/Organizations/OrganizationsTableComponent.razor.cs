////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Commerce.Organizations;

/// <summary>
/// OrganizationsTableComponent
/// </summary>
public partial class OrganizationsTableComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// Пользователь, для которого отобразить организации
    /// </summary>
    [Parameter]
    public string? UserId { get; set; }


    string? _filterUser;

    UserInfoMainModel? CurrentViewUser;

    MudTable<OrganizationModelDB> table = default!;
    bool LoadTableReady;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (string.IsNullOrWhiteSpace(UserId))
        {
            if (CurrentUserSession.IsAdmin == true)
                _filterUser = UserId;
            else
            {
                _filterUser = CurrentUserSession.UserId;
                CurrentViewUser = CurrentUserSession;
            }
        }
        else
        {
            if (CurrentUserSession.IsAdmin || CurrentUserSession.Roles?.Any(x => GlobalStaticConstantsRoles.Roles.AllHelpDeskRoles.Contains(x)) == true)
                _filterUser = UserId;
            else
                _filterUser = CurrentUserSession.UserId;
        }

        if (UserId == CurrentUserSession.UserId)
            CurrentViewUser = CurrentUserSession;
        else if (!string.IsNullOrWhiteSpace(UserId))
        {
            TResponseModel<UserInfoModel[]> user_res = await IdentityRepo.GetUsersOfIdentityAsync([UserId]);
            SnackBarRepo.ShowMessagesResponse(user_res.Messages);
            CurrentViewUser = user_res.Response?.FirstOrDefault();
        }
        await SetBusyAsync(false);
        LoadTableReady = true;
        if (table is not null)
            await table.ReloadServerData();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<OrganizationModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (!LoadTableReady || CurrentUserSession is null)
            return new TableData<OrganizationModelDB>() { TotalItems = 0, Items = [] };

        TPaginationRequestAuthModel<OrganizationsSelectRequestModel> req = new()
        {
            Payload = new()
            {
                ForUserIdentityId = _filterUser,
            },
            SenderActionUserId = CurrentUserSession.UserId,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<OrganizationModelDB> res = await CommerceRepo.OrganizationsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);

        if (res.Response is null)
            return new TableData<OrganizationModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<OrganizationModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}