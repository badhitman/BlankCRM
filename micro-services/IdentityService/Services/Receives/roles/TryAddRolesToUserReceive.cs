////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Попытка добавить роли пользователю. Если роли такой нет, то она будет создана.
/// </summary>
public class TryAddRolesToUserReceive(IIdentityTools idRepo, ILogger<TryAddRolesToUserReceive> loggerRepo)
    : IResponseReceive<UserRolesModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TryAddRolesToUserReceive;

    /// <summary>
    /// Попытка добавить роли пользователю. Если роли такой нет, то она будет создана.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(UserRolesModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.TryAddRolesToUserAsync(req, token);
    }
}