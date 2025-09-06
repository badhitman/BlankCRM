////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Telegram;

public partial class UserBadgeComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required JoinUserChatViewModel JoinUserChat { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? BadgeColor { get; set; }


    private bool _visible;
    private readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    private void OpenDialog() => _visible = true;

    private void Submit() => _visible = false;
}