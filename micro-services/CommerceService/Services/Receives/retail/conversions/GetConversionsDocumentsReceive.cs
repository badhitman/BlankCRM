////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// GetConversionsDocuments
/// </summary>
public class GetConversionsDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<ReadWalletsRetailsConversionDocumentsRequestModel?, TResponseModel<WalletConversionRetailDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetConversionsDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>?> ResponseHandleActionAsync(ReadWalletsRetailsConversionDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetConversionsDocumentsRetailAsync(req, token);
    }
}