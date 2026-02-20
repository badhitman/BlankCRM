////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд (MQ client)
/// </summary>
public interface IMQStandardClientRPC
{
    /// <summary>
    /// Удалённый вызов метода через MQ
    /// </summary>
    public Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, CancellationToken token = default);
}