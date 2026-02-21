////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RealtimeMQTTClientConfig
/// </summary>
public class RealtimeMQTTClientConfigModel : HostConfigModel
{
    /// <summary>
    /// Таймаут ожидания ответа на удалённый вызов
    /// </summary>
    public int RemoteCallTimeoutMs { get; set; } = 300000;

    /// <summary>
    /// Префикс имён очередей для ответов на удалённые команды
    /// </summary>
    public string QueueMqNamePrefixForResponse { get; set; } = "response.bus-";


    /// <inheritdoc/>
    public static RealtimeMQTTClientConfigModel BuildEmpty()
    {
        return new RealtimeMQTTClientConfigModel() { Scheme = "mqtt", Port = 1883 };
    }

    /// <inheritdoc/>
    public void Reload(RealtimeMQTTClientConfigModel other)
    {
        QueueMqNamePrefixForResponse = other.QueueMqNamePrefixForResponse;
        RemoteCallTimeoutMs = other.RemoteCallTimeoutMs;
        Scheme = other.Scheme;
        Host = other.Host;
        Port = other.Port;
    }

    /// <inheritdoc/>
    public static readonly string Configuration = "RealtimeMQTTClientConfig";
}