﻿////////////////////////////////////////////////
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
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ProductUpdateRusklimatReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ProductRusklimatModelDB? req = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await rusklimatRepo.UpdateProductAsync(req, token);
    }
}