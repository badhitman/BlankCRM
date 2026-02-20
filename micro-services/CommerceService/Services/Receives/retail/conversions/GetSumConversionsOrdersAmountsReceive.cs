////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// GetSumConversionsOrdersAmounts
/// </summary>
public class GetSumConversionsOrdersAmountsReceive(IRetailService commRepo)
    : IResponseReceive<GetSumConversionsOrdersAmountsRequestModel?, TResponseModel<decimal>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetSumConversionsOrdersAmountsRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<decimal>?> ResponseHandleActionAsync(GetSumConversionsOrdersAmountsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetSumConversionsOrdersAmountsAsync(req, token);
    }
}