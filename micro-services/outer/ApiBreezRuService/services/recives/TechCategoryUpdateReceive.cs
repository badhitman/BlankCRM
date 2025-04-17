////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// TechCategoryUpdateReceive
/// </summary>
public class TechCategoryUpdateReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<TechCategoryBreezRuModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.TechCategoryUpdateBreezReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TechCategoryBreezRuModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.TechCategoryUpdateAsync(req, token);
    }
}