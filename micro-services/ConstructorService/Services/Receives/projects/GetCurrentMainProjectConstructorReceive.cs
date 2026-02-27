////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// GetCurrentMainProjectReceive
/// </summary>
public class GetCurrentMainProjectConstructorReceive(IConstructorService conService)
    : IResponseReceive<GetCurrentMainProjectRequestModel?, TResponseModel<MainProjectViewModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetCurrentMainProjectConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel>?> ResponseHandleActionAsync(GetCurrentMainProjectRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        return await conService.GetCurrentMainProjectAsync(req, token);
    }
}