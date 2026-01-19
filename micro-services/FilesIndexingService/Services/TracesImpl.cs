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
        IMongoCollection<TraceReceive> traceReceiverRecords = mongoFs.GetCollection<TraceReceive>(nameof(TraceReceive));

        CreateIndexOptions indexOptions = new();
        IndexKeysDefinition<TraceReceive> indexKeys = Builders<TraceReceive>.IndexKeys.Ascending(x => x.ReceiverName);
        CreateIndexModel<TraceReceive> indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceReceive>.IndexKeys.Ascending(x => x.UTCTimestampInitReceive);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceReceive>.IndexKeys.Ascending(x => x.SenderActionUserId);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        await traceReceiverRecords.InsertOneAsync(TraceReceive.Build(req), cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default)
    {
        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<TraceReceive> traceReceiverRecords = mongoFs.GetCollection<TraceReceive>(nameof(TraceReceive));

        IMongoQueryable<TraceReceive> query = traceReceiverRecords
            .AsQueryable()
            .Where(x => req.Payload == null || req.Payload.ReceiversNames == null || req.Payload.ReceiversNames.Contains(x.ReceiverName))
            ;

        if (req.Payload?.PeriodStart.HasValue == true)
            query = query.Where(x => x.UTCTimestampInitReceive >= req.Payload.PeriodStart.Value.Date);

        if (req.Payload?.PeriodEnd.HasValue == true)
            query = query.Where(x => x.UTCTimestampInitReceive <= req.Payload.PeriodEnd.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

        Task<int> totalTask = query.CountAsync(cancellationToken: token);
        Task<List<TraceReceive>> itemsTask = query.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token);
        await Task.WhenAll(totalTask, itemsTask);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = totalTask.Result,
            Response = [..itemsTask.Result.Select(x=> new TraceReceive()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,
                SenderActionUserId = x.SenderActionUserId,

                RequestBody = JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            })],
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
        };
    }
}