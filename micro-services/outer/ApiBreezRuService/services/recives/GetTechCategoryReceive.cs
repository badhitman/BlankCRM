////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// GetTechCategoryReceive
/// </summary>
public class GetTechCategoryReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<TechRequestModel?, TResponseModel<List<TechCategoryRealBreezRuModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetTechCategoryBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryRealBreezRuModel>>?> ResponseHandleActionAsync(TechRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await breezRepo.GetTechCategoryAsync(req, token);
    }
}