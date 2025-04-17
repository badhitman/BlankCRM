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
    : IResponseReceive<object?, TResponseModel<CategoriesRusklimatResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetCategoriesRusklimatReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<CategoriesRusklimatResponseModel>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.GetCategoriesAsync(token);
    }
}