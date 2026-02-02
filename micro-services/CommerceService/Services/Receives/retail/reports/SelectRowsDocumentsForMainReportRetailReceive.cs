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
    : IResponseReceive<TPaginationRequestStandardModel<MainReportRequestModel>?, TPaginationResponseStandardModel<DocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectRowsRetailDocumentsForMainReportRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DocumentRetailModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<MainReportRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectRowsDocumentsForMainReportRetailAsync(req, token);
    }
}