////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Извлекает связанные логины для указанного <param ref="userId"/>
/// </summary>
public class GetUserLoginsReceive(IIdentityTools idRepo, ILogger<GetUserLoginsReceive> loggerRepo)
    : IResponseReceive<string?, TResponseModel<IEnumerable<UserLoginInfoModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetUserLoginsReceive;

    /// <summary>
    /// Извлекает связанные логины для указанного <param ref="userId"/>
    /// </summary>
    public async Task<TResponseModel<IEnumerable<UserLoginInfoModel>>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.GetUserLoginsAsync(req, token);
    }
}