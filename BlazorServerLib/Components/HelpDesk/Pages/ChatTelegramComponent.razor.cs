////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.HelpDesk.Pages;

/// <summary>
/// ChatTelegramComponent
/// </summary>
public partial class ChatTelegramComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// Chat id (db:id)
    /// </summary>
    [Parameter]
    public int? ChatId { get; set; }


    HelpDeskJournalComponent? _tab;

    void Update()
    {
        _tab?.TableRef.ReloadServerData();
    }

    void SetTab(HelpDeskJournalComponent page)
    {
        _tab = page;
    }

    ChatTelegramModelDB? Chat;
    TelegramUserBaseModel? CurrentUser;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (ChatId is null || ChatId < 1)
            return;

        await SetBusyAsync();
        Chat = await TelegramRepo.ChatTelegramReadAsync(ChatId.Value);
        IsBusyProgress = false;
        await SetBusyAsync();
        TResponseModel<TelegramUserBaseModel> get_user = await IdentityRepo.GetTelegramUserCachedInfoAsync(Chat.ChatTelegramId);
        IsBusyProgress = false;
        //SnackbarRepo.ShowMessagesResponse(get_user.Messages);
        CurrentUser = get_user.Response;
    }
}