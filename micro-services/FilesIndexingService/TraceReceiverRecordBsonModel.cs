////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLib;
using System.Runtime.Serialization;

namespace FilesIndexingService;

/// <summary>
/// TraceReceiverRecordBsonModel
/// </summary>
public class TraceReceiverRecordBsonModel : TraceReceiverRecord
{
    /// <summary>
    /// _id
    /// </summary>
    [BsonId, DataMember]
    public MongoDB.Bson.ObjectId _id { get; set; }

    /// <inheritdoc/>
    public static TraceReceiverRecordBsonModel Build(TraceReceiverRecord sender)
    {
        return new()
        {
            ReceiverName = sender.ReceiverName,
            RequestBody = sender.RequestBody,
            ResponseBody = sender.ResponseBody,
            UTCTimestampFinalReceive = sender.UTCTimestampFinalReceive,
            UTCTimestampInitReceive = sender.UTCTimestampInitReceive,
        };
    }
}