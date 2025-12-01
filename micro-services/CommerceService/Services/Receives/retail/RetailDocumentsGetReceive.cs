////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// RetailDocumentsGet
/// </summary>
public class RetailDocumentsGetReceive(IRetailService commRepo)
    : IResponseReceive<RetailDocumentsGetRequestModel?, TResponseModel<RetailDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DocumentsGetRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<RetailDocumentModelDB[]>?> ResponseHandleActionAsync(RetailDocumentsGetRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.RetailDocumentsGetAsync(req, token);
    }
}