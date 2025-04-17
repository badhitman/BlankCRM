////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// ProductsSelectReceive
/// </summary>
public class ProductsSelectReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<DaichiRequestModel?, TPaginationResponseModel<ProductDaichiModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductsSelectDaichiReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductDaichiModelDB>?> ResponseHandleActionAsync(DaichiRequestModel? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await daichiRepo.ProductsSelectAsync(req, token);
    }
}