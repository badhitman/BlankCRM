﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Проверка 2FA токена
/// </summary>
public class CheckToken2FAReceive(IIdentityTools idRepo)
    : IResponseReceive<CheckToken2FARequestModel?, TResponseModel<string>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CheckToken2FAReceive;

    /// <summary>
    /// Проверка 2FA токена
    /// </summary>
    public async Task<TResponseModel<string>?> ResponseHandleActionAsync(CheckToken2FARequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.CheckToken2FAAsync(req, token);
    }
}