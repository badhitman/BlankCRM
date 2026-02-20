////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// Удалённый вызов команд (MQTT client)
/// </summary>
public interface IMQClientExtRPC
{
    /// <summary>
    /// Удалённый вызов метода через MQTT
    /// </summary>
    public Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, KeyValuePair<string, byte[]>? propertyValue = null, CancellationToken token = default) where T : class;
}