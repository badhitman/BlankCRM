////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// ConfirmChangePhoneUser
/// </summary>
public class ConfirmChangePhoneUserReceive(IIdentityTools idRepo, ILogger<ConfirmChangePhoneUserReceive> loggerRepo)
    : IResponseReceive<TAuthRequestModel<ChangePhoneUserRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ConfirmChangePhoneUserReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestModel<ChangePhoneUserRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.ConfirmChangePhoneUserAsync(req, token);
    }
}