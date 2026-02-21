////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SharedLib;

namespace IndexingService;

/// <summary>
/// TraceRabbitActionsService
/// </summary>
public class TraceRabbitActionsService(IOptions<MongoConfigModel> mongoConf) : ITraceRabbitActionsService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveActionAsync(TraceRabbitActionRequestModel req, CancellationToken token = default)
    {
        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.TracesDataBaseSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<TraceRabbitActionRequestModel> traceReceiverRecords = mongoFs.GetCollection<TraceRabbitActionRequestModel>(nameof(TraceRabbitActionRequestModel));

        CreateIndexOptions indexOptions = new();
        IndexKeysDefinition<TraceRabbitActionRequestModel> indexKeys = Builders<TraceRabbitActionRequestModel>.IndexKeys.Ascending(x => x.ReceiverName);
        CreateIndexModel<TraceRabbitActionRequestModel> indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceRabbitActionRequestModel>.IndexKeys.Ascending(x => x.UTCTimestampInitReceive);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceRabbitActionRequestModel>.IndexKeys.Ascending(x => x.GuidSession);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceRabbitActionRequestModel>.IndexKeys.Ascending(x => x.Sender);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);
        object? _rb;
        if (req.PayloadBody is not null)
        {
            try
            {
                if (req.PayloadBody is Newtonsoft.Json.Linq.JObject)
                    _rb = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(JsonConvert.SerializeObject(req.PayloadBody));
                else if (req.PayloadBody is string)
                    _rb = req.PayloadBody;
                else if (req.PayloadBody is System.Collections.IEnumerable || req.PayloadBody.GetType().IsArray)
                    _rb = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonArray>(JsonConvert.SerializeObject(req.PayloadBody));
                else
                    _rb = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(JsonConvert.SerializeObject(req.PayloadBody));

                req.PayloadBody = _rb;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        try
        {
            await traceReceiverRecords.InsertOneAsync(TraceRabbitActionRequestModel.Build(req), cancellationToken: token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return ResponseBaseModel.CreateSuccess("Ok");
    }
}
/*
 Newtonsoft.Json.Linq.JObject
 */