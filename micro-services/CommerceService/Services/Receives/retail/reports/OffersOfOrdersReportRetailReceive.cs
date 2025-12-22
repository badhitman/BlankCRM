////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersOfOrdersReportRetail
/// </summary>
public class OffersOfOrdersReportRetailReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel>?, TPaginationResponseModel<OffersOfOrdersRetailReportRowModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OffersOfOrdersReportRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersOfOrdersRetailReportRowModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.OffersOfOrdersReportRetailAsync(req, token);
    }
}