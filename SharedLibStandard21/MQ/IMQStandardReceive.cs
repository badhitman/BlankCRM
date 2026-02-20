////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// Обработчик входящего сообщения
/// </summary>
public interface IMQStandardReceive<TRequest, TResponse> : IBaseStandardReceive
{
    /// <summary>
    /// Обработчик ответа на запрос
    /// </summary>
    public Task<TResponse> ResponseHandleActionAsync(TRequest payload, CancellationToken token = default);
}