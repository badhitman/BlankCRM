////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteRowRetailDocument
/// </summary>
public class DeleteRowRetailDocumentReceive(IRetailService commRepo)
    : IResponseReceive<DeleteRowRetailDocumentRequestModel?, TResponseModel<Guid?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>?> ResponseHandleActionAsync(DeleteRowRetailDocumentRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.DeleteRowRetailDocumentAsync(req, token);
    }
}