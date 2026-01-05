////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib;

/// <summary>
/// TraceReceiverRecord
/// </summary>
public class TraceReceiverRecord
{
    /// <inheritdoc/>
    public int TraceReceiverRecordId { get; set; }

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
    public string? RequestTypeName { get; set; }
    /// <inheritdoc/>
    public int? RequestKey { get; set; }

    /// <inheritdoc/>
    public object? ResponseBody { get; set; }
    /// <inheritdoc/>
    public string? ResponseTypeName { get; set; }
    /// <inheritdoc/>
    public int? ResponseKey { get; set; }

    /// <summary>
    /// TraceReceiverRecord
    /// </summary>
    public static TraceReceiverRecord Build(string _receiverName, string? _requestTypeName = null, object? _requestBody = null, int? _requestKey = null)
    {
        return new()
        {
            UTCTimestampInitReceive = DateTime.UtcNow,
            RequestTypeName = _requestTypeName,
            RequestBody = _requestBody,
            ReceiverName = _receiverName,
            RequestKey = _requestKey,
        };
    }

    /// <inheritdoc/>
    public TraceReceiverRecord SetResponse(object sender)
    {
        UTCTimestampFinalReceive = DateTime.UtcNow;
        ResponseBody = sender;
        ResponseTypeName = sender.GetType().Name;

        return this;
    }
}