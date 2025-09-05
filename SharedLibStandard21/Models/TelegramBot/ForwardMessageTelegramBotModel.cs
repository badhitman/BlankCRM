////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Пересылка сообщения
/// </summary>
public class ForwardMessageTelegramBotModel
{
    /// <summary>
    /// Чат назначения (Telegram id)
    /// </summary>
    /// <remarks>
    /// В этот чат будет отправлено сообщение
    /// </remarks>
    public long DestinationChatId { get; set; }

    /// <summary>
    /// Чат источник (Telegram id)
    /// </summary>
    /// <remarks>
    /// Исходный чат, из которого происходит пересылка сообщения
    /// </remarks>
    public long SourceChatId { get; set; }

    /// <summary>
    /// Пересылаемое сообщение (Telegram id)
    /// </summary>
    /// <remarks>
    /// Исходное сообщение, которое пересылается
    /// </remarks>
    public int SourceMessageId { get; set; }
}