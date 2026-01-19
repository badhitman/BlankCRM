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
    public static IServiceCollection FileIndexingRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<IndexingFileReceive, StorageFileMiddleModel, ResponseBaseModel>()
            .RegisterMqListener<SpreadsheetDocumentGetIndexReceive, TAuthRequestStandardModel<int>, TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>>()
            .RegisterMqListener<WordprocessingDocumentGetIndexReceive, TAuthRequestStandardModel<int>, TResponseModel<WordprocessingDocumentIndexingFileResponseModel>>()
            .RegisterMqListener<SaveTraceForReceiver, TraceReceiverRecord, ResponseBaseModel>()
            .RegisterMqListener<TracesSelectReceive, TPaginationRequestStandardModel<SelectTraceReceivesRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            .RegisterMqListener<TracesSelectForOrdersReceive, TPaginationRequestStandardModel<SelectTraceElementsRequestModel>, TPaginationResponseStandardModel<TraceReceiverRecord>>()
            ;
    }
}