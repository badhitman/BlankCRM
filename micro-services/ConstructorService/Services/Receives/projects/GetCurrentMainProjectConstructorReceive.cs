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
    : IResponseReceive<string?, TResponseModel<MainProjectViewModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetCurrentMainProjectConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<MainProjectViewModel?>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req))
            throw new ArgumentNullException(nameof(req));

        return await conService.GetCurrentMainProjectAsync(req, token);
    }
}