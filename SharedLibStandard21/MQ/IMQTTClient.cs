////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// Удалённый вызов команд (MQTT client)
/// </summary>
public interface IMQTTClient
{
    /// <summary>
    /// Удалённый вызов метода через MQTT
    /// </summary>
    public Task<T?> MqRemoteCallAsync<T>(string queue, object? request = null, bool waitResponse = true, KeyValuePair<string, byte[]>? propertyValue = null, CancellationToken token = default) where T : class;
}