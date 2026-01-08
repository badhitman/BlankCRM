////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Users;

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
            GlobalStaticConstantsRoles.Roles.HelpDeskUnit => "Исполнитель (hd)",
            GlobalStaticConstantsRoles.Roles.HelpDeskManager => "Manger (hd)",
            GlobalStaticConstantsRoles.Roles.HelpDeskChatsManage => "Чаты (hd)",
            GlobalStaticConstantsRoles.Roles.HelpDeskRubricsManage => "Рубрики (hd)",
            GlobalStaticConstantsRoles.Roles.CommerceManager => "Manger (com)",
            GlobalStaticConstantsRoles.Roles.CommerceClient => "Покупатель (b2b)",
            GlobalStaticConstantsRoles.Roles.RetailManage => "Manger (retail)",
            GlobalStaticConstantsRoles.Roles.RetailReports => "Reports (retail)",
            GlobalStaticConstantsRoles.Roles.GoodsManage => "Номенклатура",
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
        }
        ;

        await SetBusyAsync();
        if (value_bool && !User.Roles.Contains(roleName))
            await Act();
        else if (!value_bool && User.Roles.Contains(roleName))
        {
            req.Command = false;
            await Act();
        }

        await SetBusyAsync(false);
    }
}