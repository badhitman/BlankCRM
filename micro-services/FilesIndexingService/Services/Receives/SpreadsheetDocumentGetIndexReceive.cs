////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// SpreadsheetDocumentGetIndexReceive
/// </summary>
public class SpreadsheetDocumentGetIndexReceive(ILogger<SpreadsheetDocumentGetIndexReceive> LoggerRepo, IFilesIndexing indexingFileRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SpreadsheetDocumentGetIndexFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await indexingFileRepo.SpreadsheetDocumentGetIndexAsync(req, token);
    }
}