////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// GetCategories
/// </summary>
public class GetCategoriesReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<object?, CategoriesRusklimatResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.GetCategoriesRusklimatReceive;

    /// <inheritdoc/>
    public async Task<CategoriesRusklimatResponseModel?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.GetCategoriesAsync(token);
    }
}
