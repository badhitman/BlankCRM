////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// ProductsParamsGetReceive
/// </summary>
public class ProductsParamsGetReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<ProductParamsRequestDaichiModel?, TResponseModel<ProductsParamsDaichiBusinessResponseModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductsParamsGetDaichiReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel?>?> ResponseHandleActionAsync(ProductParamsRequestDaichiModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await daichiRepo.ProductsParamsGetAsync(payload, token);
    }
}