////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Telegram пользователи (сохранённые).
/// Все пользователи, которые когда либо писали что либо в бота - сохраняются/кэшируются в БД.
/// </summary>
public class FindUsersTelegramReceive(IIdentityTools identityRepo)
    : IResponseReceive<FindRequestModel?, TPaginationResponseModel<TelegramUserViewModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FindUsersTelegramReceive;

    /// <summary>
    /// Telegram пользователи (сохранённые).
    /// Все пользователи, которые когда либо писали что либо в бота - сохраняются/кэшируются в БД.
    /// </summary>
    public async Task<TPaginationResponseModel<TelegramUserViewModel>?> ResponseHandleActionAsync(FindRequestModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await identityRepo.FindUsersTelegramAsync(payload, token);
    }
}