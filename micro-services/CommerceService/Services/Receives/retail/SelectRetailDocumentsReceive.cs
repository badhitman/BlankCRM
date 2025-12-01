////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectRetailDocuments
/// </summary>
public class SelectRetailDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel>?, TPaginationResponseModel<RetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectRetailDocumentsAsync(req, token);
    }
}