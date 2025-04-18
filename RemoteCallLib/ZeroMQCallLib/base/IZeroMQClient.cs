////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Удалённый вызов команд (ZeroMQ client)
    /// </summary>
    public interface IZeroMQClient
    {
        /// <summary>
        /// Удалённый вызов метода через ZeroMQ
        /// </summary>
        public Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, CancellationToken token = default) where T : class;
    }
}