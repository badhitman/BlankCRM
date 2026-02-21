////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using System.Reflection;

namespace SharedLib;

/// <summary>
/// TraceRabbitActionRequestModel
/// </summary>
public class TraceRabbitActionRequestModel : TraceReceiverBaseRecord
{
    /// <inheritdoc/>
    public required string GuidSession { get; set; }

    /// <inheritdoc/>
    public required string Sender { get; set; }

    /// <inheritdoc/>
    public static TraceRabbitActionRequestModel Build(TraceRabbitActionRequestModel sender)
    {
        return new()
        {
            ReceiverName = sender.ReceiverName,
            PayloadBody = sender.PayloadBody,
            UTCTimestampInitReceive = sender.UTCTimestampInitReceive,
            GuidSession = sender.GuidSession,
            Sender = sender.Sender,
        };
    }
}