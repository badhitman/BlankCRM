////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// StockSharpClientConfig
/// </summary>
public class StockSharpClientConfigModel : HostConfigModel
{
    /// <summary>
    /// Таймаут ожидания ответа на удалённый вызов
    /// </summary>
    public int RemoteCallTimeoutMs { get; set; } = 3600000;

    /// <summary>
    /// Префикс имён очередей для ответов на удалённые команды
    /// </summary>
    public string QueueMqNamePrefixForResponse { get; set; } = "response.bus-";

    /// <summary>
    /// Автоматическое снятие заявок при остановке службы
    /// </summary>
    public bool CancelOrdersWithStop { get; set; }


    /// <inheritdoc/>
    public static StockSharpClientConfigModel BuildEmpty()
    {
        return new StockSharpClientConfigModel() { Scheme = "mqtt", Port = 1883 };
    }

    /// <inheritdoc/>
    public void Reload(StockSharpClientConfigModel other)
    {
        Port = other.Port;
        QueueMqNamePrefixForResponse = other.QueueMqNamePrefixForResponse;
        RemoteCallTimeoutMs = other.RemoteCallTimeoutMs;
        Scheme = other.Scheme;
        Host = other.Host;
        Port = other.Port;
    }
}