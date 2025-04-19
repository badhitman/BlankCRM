////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

public class StockSharpClientConfig : HostConfigModel
{
    /// <summary>
    /// Таймаут ожидания ответа на удалённый вызов
    /// </summary>
    public int RemoteCallTimeoutMs { get; set; } = 3600000;
}