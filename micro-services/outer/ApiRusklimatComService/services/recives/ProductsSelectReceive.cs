////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// ProductsSelect
/// </summary>
public class ProductsSelectReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<RusklimatRequestModel?, TPaginationResponseModel<ProductRusklimatModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductsSelectRusklimatReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductRusklimatModelDB>?> ResponseHandleActionAsync(RusklimatRequestModel? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await rusklimatRepo.ProductsSelectAsync(req, token);
    }
}