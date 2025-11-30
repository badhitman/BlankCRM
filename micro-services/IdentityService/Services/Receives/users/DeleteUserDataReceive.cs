////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Удалить Identity данные пользователя
/// </summary>
public class DeleteUserDataReceive(IIdentityTools idRepo, ILogger<DeleteUserDataReceive> loggerRepo)
    : IResponseReceive<DeleteUserDataRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteUserDataReceive;

    /// <summary>
    /// Удалить Identity данные пользователя
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeleteUserDataRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.DeleteUserDataAsync(req, token);
    }
}