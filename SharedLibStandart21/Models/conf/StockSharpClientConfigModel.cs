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

    /// <inheritdoc/>
    public static StockSharpClientConfigModel BuildEmpty()
    {
        return new StockSharpClientConfigModel() { Scheme = "tcp", Port = 5555 };
    }
}