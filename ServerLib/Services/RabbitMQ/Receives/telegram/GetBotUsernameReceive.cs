﻿using Microsoft.Extensions.Logging;
using SharedLib;
using Telegram.Bot;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить Username TelegramBot
/// </summary>
public class GetBotUsernameReceive(ITelegramBotClient _botClient, ILogger<GetBotUsernameReceive> _logger)
    : IResponseReceive<object?, string?>
{
    /// <inheritdoc/>
    public async Task<TResponseModel<string?>> ResponseHandleAction(object? payload = null)
    {
        TResponseModel<string?> res = new();
        Telegram.Bot.Types.User me;
        string msg;
        try
        {
            me = await _botClient.GetMeAsync();
        }
        catch (Exception ex)
        {
            msg = "Ошибка получения данных бота `_botClient.GetMeAsync`. error {50EE48C7-5A8A-420B-8B71-D1E2E44E48F4}";
            _logger.LogError(ex, msg);
            res.Messages.InjectException(ex);
            return res;
        }
        res.Response = me.Username;
        return res;
    }
}