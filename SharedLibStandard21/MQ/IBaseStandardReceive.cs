////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Обработчик входящего сообщения
/// </summary>
public interface IBaseStandardReceive
{
    /// <summary>
    /// Имя очереди
    /// </summary>
    public static string QueueName { get; } = string.Empty;
}