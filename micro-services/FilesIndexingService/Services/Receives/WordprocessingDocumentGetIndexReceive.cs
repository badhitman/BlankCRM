////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// WordprocessingDocumentGetIndexReceive
/// </summary>
public class WordprocessingDocumentGetIndexReceive(ILogger<WordprocessingDocumentGetIndexReceive> LoggerRepo, IFilesIndexing indexingFileRepo)
    : IResponseReceive<TAuthRequestModel<int>?, TResponseModel<WordprocessingDocumentIndexingFileResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WordprocessingDocumentGetIndexFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>?> ResponseHandleActionAsync(TAuthRequestModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await indexingFileRepo.WordprocessingDocumentGetIndexAsync(req, token);
    }
}