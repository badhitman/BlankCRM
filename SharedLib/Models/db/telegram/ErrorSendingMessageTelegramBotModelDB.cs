////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Ошибка отправки сообщения TelegramBot
/// </summary>
[Index(nameof(ChatId)), Index(nameof(IsDisabled))]
public class ErrorSendingMessageTelegramBotModelDB : ErrorSendingMessageTelegramBotStandardModel
{
    /// <summary>
    /// CreatedAtUtc
    /// </summary>
    public new required DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Chat
    /// </summary>
    public new required long ChatId { get; set; }

    /// <summary>
    /// Имя типа исключения
    /// </summary>
    public new required string? ExceptionTypeName { get; set; }

    /// <summary>
    /// Message (error)
    /// </summary>
    public new required string Message { get; set; }
}