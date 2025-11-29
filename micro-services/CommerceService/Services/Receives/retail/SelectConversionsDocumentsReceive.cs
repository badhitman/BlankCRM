////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectConversionsDocuments
/// </summary>
public class SelectConversionsDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel>?, TPaginationResponseModel<WalletConversionRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectConversionsDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectConversionsDocumentsAsync(req, token);
    }
}