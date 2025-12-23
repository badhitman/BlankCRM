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
    : IResponseReceive<TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel>?, TPaginationResponseModel<OffersRetailReportRowModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OffersOfDeliveriesReportRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.OffersOfDeliveriesReportRetailAsync(req, token);
    }
}