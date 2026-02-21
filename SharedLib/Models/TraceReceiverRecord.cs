////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// TraceReceiverRecord
/// </summary>
public class TraceReceiverRecord : TraceReceiverBaseRecord
{
    /// <summary>
    /// Завершение обработчика
    /// </summary>
    public DateTime UTCTimestampFinalReceive { get; set; }

    /// <summary>
    /// AuthorIdentity
    /// </summary>
    public string? SenderActionUserId { get; set; }

    /// <inheritdoc/>
    public object? ResponseBody { get; set; }

    /// <summary>
    /// TraceReceiverRecord 
    /// </summary>
    public static new TraceReceiverRecord Build<T>(string _receiverName, string? _authorId, T? _requestBody = null) where T : class
    {
        return new()
        {
            UTCTimestampInitReceive = DateTime.UtcNow,
            PayloadBody = _requestBody is null ? null : JObject.FromObject(_requestBody),
            ReceiverName = _receiverName.WithoutTransmissionQueueNamePrefix(),
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