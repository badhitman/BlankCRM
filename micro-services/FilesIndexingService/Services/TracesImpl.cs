////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using FilesIndexingService;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLib;
using System.Linq;

namespace FileIndexingService;

/// <summary>
/// TracesImpl
/// </summary>
public class TracesImpl(IOptions<MongoConfigModel> mongoConf) : ITracesIndexing
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SaveTraceForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default)
    {
        if (req.RequestBody is not null)
            req.RequestBody = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(JsonConvert.SerializeObject(req.RequestBody));

        if (req.ResponseBody is not null)
            req.ResponseBody = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(JsonConvert.SerializeObject(req.ResponseBody));

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<TraceReceiverRecordBsonModel> traceReceiverRecords = mongoFs.GetCollection<TraceReceiverRecordBsonModel>(nameof(TraceReceiverRecordBsonModel));

        CreateIndexOptions indexOptions = new();
        IndexKeysDefinition<TraceReceiverRecordBsonModel> indexKeys = Builders<TraceReceiverRecordBsonModel>.IndexKeys.Ascending(x => x.ReceiverName);
        CreateIndexModel<TraceReceiverRecordBsonModel> indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceReceiverRecordBsonModel>.IndexKeys.Ascending(x => x.UTCTimestampInitReceive);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        await traceReceiverRecords.InsertOneAsync(TraceReceiverRecordBsonModel.Build(req), cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default)
    {
        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<TraceReceiverRecordBsonModel> traceReceiverRecords = mongoFs.GetCollection<TraceReceiverRecordBsonModel>(nameof(TraceReceiverRecordBsonModel));

        IMongoQueryable<TraceReceiverRecordBsonModel> query = traceReceiverRecords
            .AsQueryable()
            .Where(x => req.Payload == null || req.Payload.ReceiversNames == null || req.Payload.ReceiversNames.Contains(x.ReceiverName))
            ;

        Task<int> totalTask = query.CountAsync(cancellationToken: token);
        Task<List<TraceReceiverRecordBsonModel>> itemsTask = query.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token);
        await Task.WhenAll(totalTask, itemsTask);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = totalTask.Result,
            Response = [..itemsTask.Result.Select(x=> new TraceReceiverRecordBsonModel()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,

                RequestBody = JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            })],
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
        };
    }
}