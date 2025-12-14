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
    : IResponseReceive<TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel>?, TPaginationResponseModel<DocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DocumentRetailModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectRetailDocumentsAsync(req, token);
    }
}