////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Возвращает флаг, указывающий, действителен ли данный password для указанного userId
/// </summary>
/// <returns>
/// true, если указанный password соответствует для userId, в противном случае значение false.
/// </returns>
public class CheckUserPasswordReceive(IIdentityTools idRepo)
    : IResponseReceive<IdentityPasswordModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CheckUserPasswordReceive;

    /// <summary>
    /// Возвращает флаг, указывающий, действителен ли данный password для указанного userId
    /// </summary>
    /// <returns>
    /// true, если указанный password соответствует для userId, в противном случае значение false.
    /// </returns>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IdentityPasswordModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.CheckUserPasswordAsync(req, token);
    }
}