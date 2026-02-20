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

    /// <summary>
    /// Тело входящего запроса
    /// </summary>
    public object? RequestBody { get; set; }

    /// <inheritdoc/>
    public object? ResponseBody { get; set; }

    /// <summary>
    /// TraceReceiverBaseRecord 
    /// </summary>
    public static TraceReceiverBaseRecord Build<T>(string _receiverName, string? _authorId, T? _requestBody = null) where T : class
    {
        return new()
        {
            UTCTimestampInitReceive = DateTime.UtcNow,
            RequestBody = _requestBody is null ? null : JObject.FromObject(_requestBody),
            ReceiverName = _receiverName.WithoutTransmissionQueueNamePrefix(),
        };
    }

    /// <inheritdoc/>
    public TraceReceiverBaseRecord SetResponse(object responseBody)
    {
        ResponseBody = responseBody;
        return this;
    }
}