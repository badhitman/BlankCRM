﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using Microsoft.Extensions.Options;

namespace BlazorWebLib.Components.Account;

/// <summary>
/// TelegramJoinComponent
/// </summary>
public partial class TelegramJoinComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ITelegramTransmission TelegramRemoteRepo { get; set; } = default!;

    [Inject]
    AuthenticationStateProvider AuthRepo { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;

    [Inject]
    IOptions<TelegramBotConfigModel> WebConfig { get; set; } = default!;


    /// <summary>
    /// UserId
    /// </summary>
    [Parameter]
    public string? UserId { get; set; }

    /// <summary>
    /// TelegramBotUsername
    /// </summary>
    [Parameter, EditorRequired]
    public string TelegramBotUsername { get; set; } = default!;


    UserInfoModel User = default!;

    UserTelegramBaseModel? bot_username;

    TelegramJoinAccountModelDb? TelegramJoinAccount;
    List<ResultMessage> Messages = [];
    TelegramUserBaseModel? TelegramUser;

    string About
    {
        get
        {
            if (TelegramJoinAccount is null)
                return $"Для привязки{(User.TelegramId.HasValue ? " нового" : "")} аккаунта Telegram к вашей учётной записи выпустите токен (кнопка: 'Создать токен').";

            string? tg_link = string.IsNullOrWhiteSpace(bot_username?.Username) ? null : $"<a target='_blank' href='https://t.me/{bot_username.Username}?start={TelegramJoinAccount.GuidToken}'>https://t.me/{bot_username.Username}?start={TelegramJoinAccount.GuidToken}</a>";
            return $"Вам создан токен {TelegramJoinAccount.GuidToken}. Теперь его нужно отправить в Telegram бота @{TelegramBotUsername} (или воспользоваться ссылкой: {tg_link}).";
        }
    }

    async Task SendEmailInfo() => await ReadState(true);

    async Task ReadState(bool notify_email)
    {
        await SetBusyAsync();
        TResponseModel<TelegramJoinAccountModelDb> rest = await IdentityRepo.TelegramJoinAccountStateAsync(new() { UserId = User.UserId, EmailNotify = notify_email, TelegramJoinAccountTokenLifetimeMinutes = WebConfig.Value.TelegramJoinAccountTokenLifetimeMinutes });
        IsBusyProgress = false;
        if (!rest.Success())
        {
            Messages = rest.Messages;
            throw new Exception("can`t get: TelegramJoinAccount. error {656F9472-7C6E-4A9B-81C8-E739D96E9E00}");
        }
        TelegramJoinAccount = rest.Response;
    }

    async Task CreateToken()
    {
        await SetBusyAsync();
        TResponseModel<TelegramJoinAccountModelDb> rest = await IdentityRepo.TelegramJoinAccountCreateAsync(User.UserId);
        Messages = rest.Messages;
        TelegramJoinAccount = rest.Response;
        IsBusyProgress = false;
    }

    async Task DeleteToken()
    {
        await SetBusyAsync();
        ResponseBaseModel rest = await IdentityRepo.TelegramJoinAccountDeleteActionAsync(User.UserId);
        Messages = rest.Messages;
        TResponseModel<TelegramJoinAccountModelDb> rest_state = await IdentityRepo.TelegramJoinAccountStateAsync(new() { UserId = User.UserId, EmailNotify = false, TelegramJoinAccountTokenLifetimeMinutes = WebConfig.Value.TelegramJoinAccountTokenLifetimeMinutes });//, 
        IsBusyProgress = false;
        TelegramJoinAccount = rest_state.Response;

        if (!rest_state.Success())
            Messages.AddRange(rest_state.Messages);
    }

    async Task Rest()
    {
        await SetBusyAsync();
        if (string.IsNullOrWhiteSpace(UserId))
        {
            AuthenticationState state = await AuthRepo.GetAuthenticationStateAsync();
            UserInfoMainModel user = state.User.ReadCurrentUserInfo() ?? throw new Exception();
            UserId = user.UserId;
        }
        TResponseModel<UserInfoModel[]> findUser = await IdentityRepo.GetUsersIdentityAsync([UserId]);
        Messages = findUser.Messages;
        if (!findUser.Success() || findUser.Response is null)
            throw new Exception("can`t get user data. error {590D6CFB-21E2-4976-AEC9-34192D34D2A0}");
        User = findUser.Response.Single();
    }
    /// <inheritdoc/>

    protected override async Task OnInitializedAsync()
    {
        await Rest();
        await ReadState(false);
        await SetBusyAsync();

        TResponseModel<UserTelegramBaseModel> bot_username_res = await TelegramRemoteRepo.AboutBotAsync();
        IsBusyProgress = false;
        bot_username = bot_username_res.Response;
        Messages.AddRange(bot_username_res.Messages);
        if (User.TelegramId.HasValue)
        {
            await SetBusyAsync();

            TResponseModel<TelegramUserBaseModel> rest = await IdentityRepo.GetTelegramUserAsync(User.TelegramId.Value);
            IsBusyProgress = false;
            Messages.AddRange(rest.Messages);
            TelegramUser = rest.Response;
        }
    }
}