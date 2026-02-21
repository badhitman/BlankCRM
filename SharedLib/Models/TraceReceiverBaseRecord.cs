////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// TraceReceiverBaseRecord
/// </summary>
public class TraceReceiverBaseRecord
{
    /// <summary>
    /// Инициализация обработчика
    /// </summary>
    public DateTime UTCTimestampInitReceive { get; set; }

    /// <summary>
    /// Имя обработчика
    /// </summary>
    public required string ReceiverName { get; set; }

    /// <inheritdoc/>
    public object? PayloadBody { get; set; }

    /// <summary>
    /// TraceReceiverBaseRecord 
    /// </summary>
    public static TraceReceiverBaseRecord Build<T>(string _receiverName, string? _authorId, T? _requestBody = null) where T : class
    {
        return new()
        {
            UTCTimestampInitReceive = DateTime.UtcNow,
            PayloadBody = _requestBody is null ? null : JObject.FromObject(_requestBody),
            ReceiverName = _receiverName.WithoutTransmissionQueueNamePrefix(),
        };
    }
}