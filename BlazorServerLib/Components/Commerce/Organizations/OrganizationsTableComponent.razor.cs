﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;
using SharedLib;

namespace BlazorWebLib.Components.Commerce.Organizations;

/// <summary>
/// OrganizationsTableComponent
/// </summary>
public partial class OrganizationsTableComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    ICommerceTransmission CommerceRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// Пользователь, для которого отобразить организации
    /// </summary>
    [Parameter]
    public string? UserId { get; set; }


    string? _filterUser;

    UserInfoMainModel? CurrentViewUser;

    private MudTable<OrganizationModelDB> table = default!;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
        if (string.IsNullOrWhiteSpace(UserId))
        {
            if (CurrentUserSession!.IsAdmin)
                _filterUser = UserId;
            else
            {
                _filterUser = CurrentUserSession!.UserId;
                CurrentViewUser = CurrentUserSession!;
            }
        }
        else
        {
            if (CurrentUserSession!.IsAdmin || CurrentUserSession!.Roles?.Any(x => GlobalStaticConstantsRoles.Roles.AllHelpDeskRoles.Contains(x)) == true)
                _filterUser = UserId;
            else
                _filterUser = CurrentUserSession!.UserId;
        }

        if (UserId == CurrentUserSession!.UserId)
            CurrentViewUser = CurrentUserSession!;
        else if (!string.IsNullOrWhiteSpace(UserId))
        {
            await SetBusyAsync();
            TResponseModel<UserInfoModel[]> user_res = await IdentityRepo.GetUsersIdentityAsync([UserId]);
            SnackBarRepo.ShowMessagesResponse(user_res.Messages);
            CurrentViewUser = user_res.Response?.FirstOrDefault();
            await SetBusyAsync(false);
        }
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<OrganizationModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        if (CurrentUserSession is null)
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
        TPaginationResponseModel<OrganizationModelDB> res = await CommerceRepo.OrganizationsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);

        if (res.Response is null)
            return new TableData<OrganizationModelDB>() { TotalItems = 0, Items = [] };

        return new TableData<OrganizationModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}