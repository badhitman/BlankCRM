////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

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
    public static TraceReceiverRecord Build(string _receiverName, object? _requestBody = null)
    {
        if (_receiverName.StartsWith(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix))
            _receiverName = _receiverName[GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix.Length..];

        return new()
        {
            UTCTimestampInitReceive = DateTime.UtcNow,
            RequestBody = _requestBody,
            ReceiverName = _receiverName,
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