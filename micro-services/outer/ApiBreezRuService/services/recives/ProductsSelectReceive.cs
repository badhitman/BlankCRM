////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// ProductsSelectReceive
/// </summary>
public class ProductsSelectReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<BreezRequestModel?, TPaginationResponseModel<ProductViewBreezRuModeld>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductsSelectBreezReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductViewBreezRuModeld>?> ResponseHandleActionAsync(BreezRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.ProductsSelectAsync(req, token);
    }
}