////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.firebase;

/// <summary>
/// Get Firebase config
/// </summary>
public class GetFirebaseConfigReceive(IOptions<FirebaseSDKConfigModel> config)
    : IResponseReceive<object?, TResponseModel<FirebaseSDKConfigModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetFirebaseConfigReceive;

    /// <inheritdoc/>
    public Task<TResponseModel<FirebaseSDKConfigModel>?> ResponseHandleActionAsync(object? req, CancellationToken token = default)
    {
#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        return Task.FromResult(new TResponseModel<FirebaseSDKConfigModel>() { Response = config.Value });
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
    }
}