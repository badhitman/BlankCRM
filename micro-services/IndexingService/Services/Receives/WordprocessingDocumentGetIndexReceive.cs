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
public class WordprocessingDocumentGetIndexReceive(IIndexingServive indexingFileRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, TResponseModel<WordprocessingDocumentIndexingFileResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WordprocessingDocumentGetIndexFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await indexingFileRepo.WordprocessingDocumentGetIndexAsync(req, token);
    }
}