////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Обработчик входящего сообщения
    /// </summary>
    public interface IZeroMQReceive<TRequest, TResponse>
    {
        /// <summary>
        /// Обработчик ответа на запрос
        /// </summary>
        public Task<TResponse> ResponseHandleActionAsync(TRequest payload, CancellationToken token = default);

        /// <summary>
        /// Имя очереди
        /// </summary>
        public static string QueueName { get; } = string.Empty;
    }
}