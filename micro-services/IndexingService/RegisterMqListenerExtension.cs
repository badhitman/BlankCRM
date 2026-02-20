////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;
using Transmission.Receives.indexing;

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
            .RegisterListenerRabbitMQ<TracesSelectReceive, TPaginationRequestStandardModel<SelectTraceReceivesRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForOrdersRetailReceive, TPaginationRequestStandardModel<SelectTraceElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForDeliveriesRetailReceive, TPaginationRequestStandardModel<SelectTraceElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForConversionsRetailReceive, TPaginationRequestStandardModel<SelectTraceElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterListenerRabbitMQ<TracesSelectForPaymentsRetailReceive, TPaginationRequestStandardModel<SelectTraceElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            ;
    }
}