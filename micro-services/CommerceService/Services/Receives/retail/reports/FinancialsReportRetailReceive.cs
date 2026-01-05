////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// FinancialsReportRetail
/// </summary>
public class FinancialsReportRetailReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel>?, TPaginationResponseStandardModel<WalletRetailReportRowModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FinancialsReportRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletRetailReportRowModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.FinancialsReportRetailAsync(req, token);
    }
}