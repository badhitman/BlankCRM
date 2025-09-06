////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib;

/// <summary>
/// Типы сообщений
/// </summary>
public enum TelegramMessagesTypesEnum
{
    /// <summary>
    /// Стандартное текстовое сообщение
    /// </summary>
    [Description("Text")]
    TextMessage,

    /// <summary>
    /// CallbackQuery
    /// </summary>
    [Description("Callback")]
    CallbackQuery
}
