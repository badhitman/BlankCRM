////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using SharedLib;

namespace IndexingService;

/// <summary>
/// TracesImpl
/// </summary>
public class HistoryImpl(IOptions<MongoConfigModel> mongoConf) : IHistoryIndexing
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

        if (req.Payload?.Start.HasValue == true)
            query = query.Where(x => x.UTCTimestampInitReceive >= req.Payload.Start.Value.Date);

        if (req.Payload?.End.HasValue == true)
            query = query.Where(x => x.UTCTimestampInitReceive <= req.Payload.End.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));

        IOrderedMongoQueryable<TraceReceive> oQuery = req.SortingDirection switch
        {
            DirectionsEnum.Up => query.OrderBy(x => x.UTCTimestampInitReceive),
            DirectionsEnum.Down => query.OrderByDescending(x => x.UTCTimestampInitReceive),
            DirectionsEnum.None => query.OrderBy(x => x.ReceiverName),
            _ => query.OrderByDescending(x => x.UTCTimestampInitReceive)
        };

        Task<int> totalTask = query.CountAsync(cancellationToken: token);
        Task<List<TraceReceive>> itemsTask = oQuery.Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToListAsync(cancellationToken: token);
        await Task.WhenAll(totalTask, itemsTask);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = totalTask.Result,
            Response = [..itemsTask.Result.Select(x => new TraceReceive()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,
                SenderActionUserId = x.SenderActionUserId,

                RequestBody = x.RequestBody is null ? null : JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = x.ResponseBody is null ? null : JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            })],
            SortBy = req.SortBy,
            SortingDirection = req.SortingDirection,
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectForOrdersRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new()
                      {
                           TypeMessage = MessagesTypesEnum.Error,
                            Text = "req.Payload is null"
                      }]
                }
            };

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<BsonDocument> collection = mongoFs.GetCollection<BsonDocument>(nameof(TraceReceive));

        FilterDefinitionBuilder<BsonDocument> filterBuilder = Builders<BsonDocument>.Filter;
        FilterDefinition<BsonDocument> filter = filterBuilder.Empty;

        filter = filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateOrderStatusDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderStatusRetailDocumentModelDB.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteOrderStatusDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(TResponseModel<>.Response)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateRowDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RowOfRetailOrderDocumentModelDB.OrderId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(DocumentRetailModelDB.Id)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrderStatusDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderStatusRetailDocumentModelDB.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRowDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(DeleteRowRetailDocumentResponseModel.Response)}.{nameof(RowOfRetailOrderDocumentModelDB.OrderId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateRowDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RowOfRetailOrderDocumentModelDB.OrderId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(TResponseModel<>.Response)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateConversionOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderConversionAmountModel.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreatePaymentOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(PaymentOrderRetailLinkModelDB.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RetailOrderDeliveryLinkModelDB.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteConversionOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderConversionModel.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeletePaymentOrderLinkDocumentReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderPaymentModel.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderDeliveryModel.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdatePaymentOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(PaymentOrderRetailLinkModelDB.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RetailOrderDeliveryLinkModelDB.OrderDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConversionOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderConversionAmountModel.OrderDocumentId)}", req.Payload.FilterId))
         ;

        IFindFluent<BsonDocument, BsonDocument> findDescriptor = collection
            .Find(filter);

        IOrderedFindFluent<BsonDocument, BsonDocument> filteredSource = req.SortingDirection switch
        {
            DirectionsEnum.Up => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.Down => findDescriptor.SortByDescending(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.None => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)]),
            _ => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)])
        };

        List<BsonDocument> records = await filteredSource.Skip(req.PageNum * req.PageSize).Limit(req.PageSize).ToListAsync(cancellationToken: token);
        long count = await collection.CountDocumentsAsync(filter, cancellationToken: token);
        List<TraceReceive> recordsPoco = [.. records.Select(x => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TraceReceive>(x))];

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = (int)await collection.CountDocumentsAsync(filter, cancellationToken: token),
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            Response = [.. recordsPoco.Select(x => new TraceReceive()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,
                SenderActionUserId = x.SenderActionUserId,

                RequestBody = JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            })],
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectForDeliveriesRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new()
                      {
                           TypeMessage = MessagesTypesEnum.Error,
                            Text = "req.Payload is null"
                      }]
                }
            };

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<BsonDocument> collection = mongoFs.GetCollection<BsonDocument>(nameof(TraceReceive));

        FilterDefinitionBuilder<BsonDocument> filterBuilder = Builders<BsonDocument>.Filter;
        FilterDefinition<BsonDocument> filter = filterBuilder.Empty;

        filter = filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(DeliveryStatusRetailDocumentModelDB.DeliveryDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryStatusDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(DeleteDeliveryStatusDocumentRequestModel.DeleteDeliveryStatusDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateRowOfDeliveryDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RowOfDeliveryRetailDocumentModelDB.DocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(DeliveryDocumentRetailModelDB.Id)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryStatusDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(DeliveryStatusRetailDocumentModelDB.DeliveryDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRowOfDeliveryDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(DocumentNewVersionResponseModel.Response)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateRowOfDeliveryDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RowOfDeliveryRetailDocumentModelDB.DocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(TResponseModel<>.Response)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RetailOrderDeliveryLinkModelDB.DeliveryDocumentId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderDeliveryModel.DeliveryId)}", req.Payload.FilterId))
            | filterBuilder.And(
                filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(RetailOrderDeliveryLinkModelDB.DeliveryDocumentId)}", req.Payload.FilterId))
         ;

        IFindFluent<BsonDocument, BsonDocument> findDescriptor = collection
            .Find(filter);

        IOrderedFindFluent<BsonDocument, BsonDocument> filteredSource = req.SortingDirection switch
        {
            DirectionsEnum.Up => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.Down => findDescriptor.SortByDescending(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.None => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)]),
            _ => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)])
        };

        List<BsonDocument> records = await filteredSource.Skip(req.PageNum * req.PageSize).Limit(req.PageSize).ToListAsync(cancellationToken: token);
        long count = await collection.CountDocumentsAsync(filter, cancellationToken: token);
        List<TraceReceive> recordsPoco = [.. records.Select(x => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TraceReceive>(x))];

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = (int)await collection.CountDocumentsAsync(filter, cancellationToken: token),
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            Response = [.. recordsPoco.Select(x => new TraceReceive()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,
                SenderActionUserId = x.SenderActionUserId,

                RequestBody = x.RequestBody is null ? null : JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = x.ResponseBody is null ? null : JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            })],
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectForConversionsRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new()
                    {
                        TypeMessage = MessagesTypesEnum.Error,
                        Text = "req.Payload is null",
                    }]
                }
            };

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<BsonDocument> collection = mongoFs.GetCollection<BsonDocument>(nameof(TraceReceive));

        FilterDefinitionBuilder<BsonDocument> filterBuilder = Builders<BsonDocument>.Filter;
        FilterDefinition<BsonDocument> filter = filterBuilder.Empty;

        filter = filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConversionDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(WalletConversionRetailDocumentModelDB.Id)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateConversionDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(TResponseModel<>.Response)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteToggleConversionRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(DeleteToggleConversionRequestModel.DeleteToggleConversionId)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreateConversionOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderConversionAmountModel.ConversionDocumentId)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeleteConversionOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderConversionModel.ConversionDocumentId)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConversionOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderConversionAmountModel.ConversionDocumentId)}", req.Payload.FilterId))
                 ;

        IFindFluent<BsonDocument, BsonDocument> findDescriptor = collection
            .Find(filter);

        IOrderedFindFluent<BsonDocument, BsonDocument> filteredSource = req.SortingDirection switch
        {
            DirectionsEnum.Up => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.Down => findDescriptor.SortByDescending(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.None => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)]),
            _ => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)])
        };

        List<BsonDocument> records = await filteredSource.Skip(req.PageNum * req.PageSize).Limit(req.PageSize).ToListAsync(cancellationToken: token);
        long count = await collection.CountDocumentsAsync(filter, cancellationToken: token);
        List<TraceReceive> recordsPoco = [.. records.Select(x => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TraceReceive>(x))];

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = (int)await collection.CountDocumentsAsync(filter, cancellationToken: token),
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            Response = [.. recordsPoco.Select(x => new TraceReceive()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,
                SenderActionUserId = x.SenderActionUserId,

                RequestBody = x.RequestBody is null ? null : JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = x.ResponseBody is null ? null : JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            })],
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectForPaymentsRetailAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new()
                    {
                        TypeMessage = MessagesTypesEnum.Error,
                        Text = "req.Payload is null",
                    }]
                }
            };

        IMongoDatabase mongoFs = new MongoClient(mongoConf.Value.ToString()).GetDatabase($"{mongoConf.Value.BusTracesSystemName}{GlobalStaticConstantsTransmission.GetModePrefix}");
        IMongoCollection<BsonDocument> collection = mongoFs.GetCollection<BsonDocument>(nameof(TraceReceive));

        FilterDefinitionBuilder<BsonDocument> filterBuilder = Builders<BsonDocument>.Filter;
        FilterDefinition<BsonDocument> filter = filterBuilder.Empty;

        filter = filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdatePaymentDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(PaymentRetailDocumentModelDB.Id)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreatePaymentDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.ResponseBody)}._v.{nameof(TResponseModel<>.Response)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.CreatePaymentOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(PaymentOrderRetailLinkModelDB.PaymentDocumentId)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.DeletePaymentOrderLinkDocumentReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(OrderPaymentModel.PaymentId)}", req.Payload.FilterId))
                 | filterBuilder.And(
                     filterBuilder.Eq(nameof(TraceReceiverRecord.ReceiverName), GlobalStaticConstantsTransmission.TransmissionQueues.UpdatePaymentOrderLinkDocumentRetailReceive.WithoutTransmissionQueueNamePrefix()),
                     filterBuilder.Eq($"{nameof(TraceReceiverRecord.RequestBody)}._v.{nameof(PaymentOrderRetailLinkModelDB.PaymentDocumentId)}", req.Payload.FilterId))
                 ;

        IFindFluent<BsonDocument, BsonDocument> findDescriptor = collection
            .Find(filter);

        IOrderedFindFluent<BsonDocument, BsonDocument> filteredSource = req.SortingDirection switch
        {
            DirectionsEnum.Up => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.Down => findDescriptor.SortByDescending(d => d[nameof(TraceReceiverRecord.UTCTimestampInitReceive)]),
            DirectionsEnum.None => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)]),
            _ => findDescriptor.SortBy(d => d[nameof(TraceReceiverRecord.ReceiverName)])
        };

        List<BsonDocument> records = await filteredSource.Skip(req.PageNum * req.PageSize).Limit(req.PageSize).ToListAsync(cancellationToken: token);
        long count = await collection.CountDocumentsAsync(filter, cancellationToken: token);
        List<TraceReceive> recordsPoco = [.. records.Select(x => MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TraceReceive>(x))];

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            TotalRowsCount = (int)await collection.CountDocumentsAsync(filter, cancellationToken: token),
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            Response = [.. recordsPoco.Select(x => new TraceReceive()
            {
                UTCTimestampFinalReceive = x.UTCTimestampFinalReceive,
                UTCTimestampInitReceive = x.UTCTimestampInitReceive,

                ReceiverName = x.ReceiverName,
                SenderActionUserId = x.SenderActionUserId,

                RequestBody = x.RequestBody is null ? null : JObject.Parse(x.RequestBody.ToBsonDocument().ToJson()),
                ResponseBody = x.ResponseBody is null ? null : JObject.Parse(x.ResponseBody.ToBsonDocument().ToJson()),
            }).OrderByDescending(x => x.UTCTimestampInitReceive)],
        };
    }
}