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
    : IResponseReceive<TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel>?, TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectConversionsDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletConversionRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectConversionsDocumentsRetailAsync(req, token);
    }
}