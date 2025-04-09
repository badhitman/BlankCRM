////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Обработчик входящего сообщения
/// </summary>
public interface IResponseReceive<TRequest, TResponse>
{
    /// <summary>
    /// Обработчик ответа на запрос
    /// </summary>
    public Task<TResponse?> ResponseHandleActionAsync(TRequest? payload, CancellationToken token = default);

    /// <summary>
    /// Имя очереди
    /// </summary>
    public virtual static string QueueName { get; } = string.Empty;
}