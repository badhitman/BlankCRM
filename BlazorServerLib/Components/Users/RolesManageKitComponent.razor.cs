﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Users;

/// <summary>
/// RolesManageKitComponent
/// </summary>
public partial class RolesManageKitComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// RolesManageKit
    /// </summary>
    [Parameter, EditorRequired]
    public required IEnumerable<string> RolesManageKit { get; set; }

    /// <summary>
    /// User
    /// </summary>
    [Parameter, EditorRequired]
    public required UserInfoModel User { get; set; }


    bool IsShow { get; set; }

    static string RoleDescription(string name)
    {
        return name switch
        {
            GlobalStaticConstantsRoles.Roles.HelpDeskTelegramBotUnit => "Исполнитель",
            GlobalStaticConstantsRoles.Roles.HelpDeskTelegramBotManager => "Управляющий",
            GlobalStaticConstantsRoles.Roles.HelpDeskTelegramBotChatsManage => "Чаты",
            GlobalStaticConstantsRoles.Roles.HelpDeskTelegramBotRubricsManage => "Рубрики",
            GlobalStaticConstantsRoles.Roles.CommerceManager => "Менеджер",
            GlobalStaticConstantsRoles.Roles.CommerceClient => "Покупатель",
            _ => name
        };
    }

    bool IsChecked(string role_name)
    {
        return User.Roles?.Contains(role_name) == true;
    }

    async Task ChangeUserRole(ChangeEventArgs e, string roleName)
    {
        bool value_bool = e.Value is not null && (bool)e.Value == true;
        User.Roles ??= [];

        SetRoleForUserRequestModel req = new()
        {
            RoleName = roleName,
            UserIdentityId = User.UserId,
            Command = value_bool,
        };

        async Task Act()
        {
            TResponseModel<string[]> res = await IdentityRepo.SetRoleForUserAsync(req);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (!res.Success() || res.Response is null)
                return;

            User.Roles.Clear();
            User.Roles.AddRange(res.Response);
        };

        await SetBusyAsync();
        if (value_bool && !User.Roles.Contains(roleName))
            await Act();
        else if (!value_bool && User.Roles.Contains(roleName))
        {
            req.Command = false;
            await Act();
        }
        IsBusyProgress = false;
    }
}