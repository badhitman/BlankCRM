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
public class TraceReceive : TraceReceiverRecord
{
    /// <summary>
    /// _id
    /// </summary>
    [BsonId, DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    /// <inheritdoc/>
    public static TraceReceive Build(TraceReceiverRecord sender)
    {
        return new()
        {
            SenderActionUserId = sender.SenderActionUserId,
            ReceiverName = sender.ReceiverName,
            RequestBody = sender.RequestBody,
            ResponseBody = sender.ResponseBody,
            UTCTimestampFinalReceive = sender.UTCTimestampFinalReceive,
            UTCTimestampInitReceive = sender.UTCTimestampInitReceive,
        };
    }
}