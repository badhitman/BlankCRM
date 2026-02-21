////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using SharedLib;

namespace IndexingService;

/// <summary>
/// TraceReceive
/// </summary>
public class TraceReceive : TraceReceiverBaseRecord
{
    /// <summary>
    /// _id
    /// </summary>
    [BsonId, DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    /// <inheritdoc/>
    public static TraceReceive Build(TraceReceiverBaseRecord sender)
    {
        return new()
        {
            ReceiverName = sender.ReceiverName,
            PayloadBody = sender.PayloadBody,
            UTCTimestampInitReceive = sender.UTCTimestampInitReceive,
        };
    }
}