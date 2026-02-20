////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
        IMongoCollection<TraceReceive> traceReceiverRecords = mongoFs.GetCollection<TraceReceive>(nameof(TraceReceive));

        CreateIndexOptions indexOptions = new();
        IndexKeysDefinition<TraceReceive> indexKeys = Builders<TraceReceive>.IndexKeys.Ascending(x => x.ReceiverName);
        CreateIndexModel<TraceReceive> indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        indexKeys = Builders<TraceReceive>.IndexKeys.Ascending(x => x.UTCTimestampInitReceive);
        indexModel = new(indexKeys, indexOptions);
        await traceReceiverRecords.Indexes.CreateOneAsync(indexModel, cancellationToken: token);

        await traceReceiverRecords.InsertOneAsync(TraceReceive.Build(req), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }
}
