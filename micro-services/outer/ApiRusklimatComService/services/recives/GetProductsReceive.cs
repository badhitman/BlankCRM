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
    : IResponseReceive<PaginationRequestModel?, ProductsRusklimatResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetProductsRusklimatReceive;

    /// <inheritdoc/>
    public async Task<ProductsRusklimatResponseModel?> ResponseHandleActionAsync(PaginationRequestModel? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await rusklimatRepo.GetProductsAsync(req,token);
    }
}