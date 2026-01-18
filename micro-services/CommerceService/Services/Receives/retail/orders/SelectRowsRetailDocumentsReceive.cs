////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectRowsRetailDocuments
/// </summary>
public class SelectRowsRetailDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel>?, TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectRowsDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectRowsRetailDocumentsAsync(req, token);
    }
}