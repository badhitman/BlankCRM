////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectConversionsOrdersDocumentsLinks
/// </summary>
public class SelectConversionsOrdersDocumentsLinksReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel>?, TPaginationResponseModel<ConversionOrderRetailLinkModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectConversionsOrdersDocumentsLinksRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ConversionOrderRetailLinkModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectConversionsOrdersDocumentsLinksRetailAsync(req, token);
    }
}