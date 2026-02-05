////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// StockSharpClientConfig
/// </summary>
public class MQTTClientConfigModel : HostConfigModel
{
    /// <summary>
    /// Таймаут ожидания ответа на удалённый вызов
    /// </summary>
    public int RemoteCallTimeoutMs { get; set; } = 3600000;

    /// <summary>
    /// Префикс имён очередей для ответов на удалённые команды
    /// </summary>
    public string QueueMqNamePrefixForResponse { get; set; } = "response.bus-";


    /// <inheritdoc/>
    public static MQTTClientConfigModel BuildEmpty()
    {
        return new MQTTClientConfigModel() { Scheme = "mqtt", Port = 1883 };
    }

    /// <inheritdoc/>
    public void Reload(MQTTClientConfigModel other)
    {
        Port = other.Port;
        QueueMqNamePrefixForResponse = other.QueueMqNamePrefixForResponse;
        RemoteCallTimeoutMs = other.RemoteCallTimeoutMs;
        Scheme = other.Scheme;
        Host = other.Host;
        Port = other.Port;
    }
}

/// <summary>
/// MQTTClientConfigMainModel
/// </summary>
public partial class MQTTClientConfigMainModel : MQTTClientConfigModel
{
    /// <inheritdoc/>
    public new static MQTTClientConfigMainModel BuildEmpty()
    {
        return new MQTTClientConfigMainModel() { Scheme = "mqtt", Port = 1883 };
    }
}
 