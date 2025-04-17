////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;
using static SharedLib.GlobalStaticConstantsTransmission;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// ProductUpdateReceive
/// </summary>
public class ProductUpdateReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<ProductBreezRuModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => TransmissionQueues.ProductUpdateBreezReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ProductBreezRuModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.ProductUpdateAsync(req,token);
    }
}

/// <summary>
/// CategoryUpdateReceive
/// </summary>
public class CategoryUpdateReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<CategoryBreezRuModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => TransmissionQueues.CategoryUpdateBreezReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(CategoryBreezRuModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.CategoryUpdateAsync(req, token);
    }
}