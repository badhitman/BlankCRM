////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// ProductUpdateReceive
/// </summary>
public class ProductUpdateReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<ProductDaichiModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductUpdateDaichiReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ProductDaichiModelDB? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await daichiRepo.ProductUpdateAsync(req, token);
    }
}