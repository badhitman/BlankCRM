////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// GetProducts
/// </summary>
public class GetProductsReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<RusklimatPaginationRequestModel?, TResponseModel<ProductsRusklimatResponseModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetProductsRusklimatReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsRusklimatResponseModel?>?> ResponseHandleActionAsync(RusklimatPaginationRequestModel? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await rusklimatRepo.GetProductsAsync(req,token);
    }
}