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
            .RegisterMqListener<SpreadsheetDocumentGetIndexReceive, TAuthRequestModel<int>, TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>>()
            .RegisterMqListener<WordprocessingDocumentGetIndexReceive, TAuthRequestModel<int>, TResponseModel<WordprocessingDocumentIndexingFileResponseModel>>()
            ;
    }
}