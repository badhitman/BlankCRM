////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// ProductsGetReceive
/// </summary>
public class ProductsGetReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<ProductsRequestDaichiModel?, TResponseModel<ProductsDaichiBusinessResultModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ProductsGetDaichiReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsDaichiBusinessResultModel>?> ResponseHandleActionAsync(ProductsRequestDaichiModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await daichiRepo.ProductsGetAsync(payload, token);
    }
}
