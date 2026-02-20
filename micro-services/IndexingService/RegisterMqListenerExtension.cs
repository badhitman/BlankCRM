////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.indexing;
using Transmission.Receives.realtime;

namespace FileIndexingService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection IndexingServiceRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterListenerRabbitMQ<IndexingFileReceive, StorageFileMiddleModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SpreadsheetDocumentGetIndexReceive, TAuthRequestStandardModel<int>, TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>>()
            .RegisterListenerRabbitMQ<WordprocessingDocumentGetIndexReceive, TAuthRequestStandardModel<int>, TResponseModel<WordprocessingDocumentIndexingFileResponseModel>>()
            .RegisterListenerRabbitMQ<SaveTraceForReceiver, TraceReceiverRecord, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<TracesSelectReceive, TPaginationRequestStandardModel<SelectHistoryReceivesRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForOrdersRetailReceive, TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForDeliveriesRetailReceive, TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForConversionsRetailReceive, TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForPaymentsRetailReceive, TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()

            .RegisterListenerNetMQ<TraceRabbitActionReceive, MessageWebChatModelDB, TResponseModel<int>>()
            ;
    }
}