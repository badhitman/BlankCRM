////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд (RabbitMq client)
/// </summary>
public interface IRabbitClient
{
    /// <summary>
    /// Удалённый вызов метода через MQ
    /// </summary>
    public Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, CancellationToken token = default);
}