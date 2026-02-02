////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectRowsDocumentsForMainReportRetail
/// </summary>
public class SelectRowsDocumentsForMainReportRetailReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<MainReportRequestModel>?, TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OffersOfDeliveriesReportRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RowOfRetailOrderDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<MainReportRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectRowsDocumentsForMainReportRetailAsync(req, token);
    }
}