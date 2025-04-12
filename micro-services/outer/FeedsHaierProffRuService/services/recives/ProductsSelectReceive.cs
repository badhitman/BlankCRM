////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.HaierProff;

/// <summary>
/// ProductsSelect
/// </summary>
public class ProductsSelectReceive(IFeedsHaierProffRuService haierRepo)
    : IResponseReceive<HaierRequestModel?, TPaginationResponseModel<ProductHaierModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ProductsSelectHaierProffReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductHaierModelDB>?> ResponseHandleActionAsync(HaierRequestModel? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await haierRepo.ProductsSelectAsync(req,token);
    }
}