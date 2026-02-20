////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// SpreadsheetDocumentGetIndexReceive
/// </summary>
public class SpreadsheetDocumentGetIndexReceive( IIndexingServive indexingFileRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SpreadsheetDocumentGetIndexFileReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await indexingFileRepo.SpreadsheetDocumentGetIndexAsync(req, token);
    }
}