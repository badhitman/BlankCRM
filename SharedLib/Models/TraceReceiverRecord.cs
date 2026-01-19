////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// TraceReceiverRecord
/// </summary>
public class TraceReceiverRecord
{
    /// <summary>
    /// Инициализация обработчика
    /// </summary>
    public DateTime UTCTimestampInitReceive { get; set; }
    /// <summary>
    /// Завершение обработчика
    /// </summary>
    public DateTime UTCTimestampFinalReceive { get; set; }

    /// <summary>
    /// AuthorIdentity
    /// </summary>
    public string? SenderActionUserId { get; set; }

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
    /// TraceReceiverRecord 
    /// </summary>
    public static TraceReceiverRecord Build<T>(string _receiverName, string? _authorId, T? _requestBody = null) where T : class
    {
        if (_receiverName.StartsWith(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix))
            _receiverName = _receiverName[GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.Length..];

        return new()
        {
            UTCTimestampInitReceive = DateTime.UtcNow,
            RequestBody = _requestBody is null ? null : JObject.FromObject(_requestBody),
            ReceiverName = _receiverName,
            SenderActionUserId = _authorId,
        };
    }

    /// <inheritdoc/>
    public TraceReceiverRecord SetResponse(object responseBody)
    {
        UTCTimestampFinalReceive = DateTime.UtcNow;
        ResponseBody = responseBody;

        return this;
    }
}