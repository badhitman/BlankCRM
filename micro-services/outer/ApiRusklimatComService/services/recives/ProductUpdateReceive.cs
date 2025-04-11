////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// ProductUpdateReceive
/// </summary>
public class ProductUpdateReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<ProductRusklimatModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ProductUpdateRusklimatReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ProductRusklimatModelDB? payload = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.UpdateProductAsync(payload, token);
    }
}