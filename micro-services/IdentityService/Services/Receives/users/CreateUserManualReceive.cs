////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// CreateUserManual
/// </summary>
public class CreateUserManualReceive(IIdentityTools idRepo, ILogger<CreateUserManualReceive> loggerRepo)
    : IResponseReceive<TAuthRequestStandardModel<UserInfoBaseModel>?, TResponseModel<string>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateManualUserReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<string>?> ResponseHandleActionAsync(TAuthRequestStandardModel<UserInfoBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.CreateUserManualAsync(req, token);
    }
}