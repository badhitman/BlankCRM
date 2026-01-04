////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateRowRetailDocument
/// </summary>
public class CreateRowRetailDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RowOfRetailOrderDocumentModelDB?, TResponseModel<KeyValuePair<int, Guid>?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<KeyValuePair<int, Guid>?>?> ResponseHandleActionAsync(RowOfRetailOrderDocumentModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.CreateRowRetailDocumentAsync(req, token);
    }
}