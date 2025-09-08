////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

/// <summary>
/// UserBadgeComponent
/// </summary>
public partial class UserBadgeComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramBotStandardTransmission TelegramRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required JoinUserChatViewModel JoinUserChat { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? BadgeColor { get; set; }


    bool _visible;
    readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    IReadOnlyCollection<TelegramUsersRolesEnum> _selected = [];
    IReadOnlyCollection<TelegramUsersRolesEnum> Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            InvokeAsync(SavePermissions);
        }
    }


    async Task SavePermissions()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await TelegramRepo.UserTelegramPermissionUpdateAsync(new UserTelegramPermissionSetModel()
        {
            Roles = [.. Selected],
            UserId = JoinUserChat.UserId,
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        List<UserTelegramViewModel> res = await TelegramRepo.UsersReadTelegramAsync([JoinUserChat.UserId]);
        await SetBusyAsync(false);
        if (res.Count == 1)
        {
            _selected = [.. res[0]!.UserRoles!.Select(x => x.Role)];
        }
    }

    void OpenDialog() => _visible = true;

    void Submit() => _visible = false;
}