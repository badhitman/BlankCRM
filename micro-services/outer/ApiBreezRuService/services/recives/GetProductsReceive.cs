﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// GetProductsReceive
/// </summary>
public class GetProductsReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<object?, TResponseModel<List<ProductRealBreezRuModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetProductsBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductRealBreezRuModel>>?> ResponseHandleActionAsync(object? payload, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.GetProductsAsync(token);
    }
}