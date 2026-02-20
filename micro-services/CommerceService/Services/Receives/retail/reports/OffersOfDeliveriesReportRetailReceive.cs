////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersOfDeliveriesReport
/// </summary>
public class OffersOfDeliveriesReportRetailReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel>?, TPaginationResponseStandardModel<OffersRetailReportRowModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OffersOfDeliveriesReportRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OffersRetailReportRowModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.OffersOfDeliveriesReportRetailAsync(req, token);
    }
}