////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// TelegramBotStandardTransmission
/// </summary>
public partial class TelegramBotStandardTransmission(IMQTTClient mqClient) : ITelegramBotStandardTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<UserTelegramBaseModel>> AboutBotAsync(CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<UserTelegramBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotUsernameReceive,  token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendTextMessageTelegramReceive, req, waitResponse: false, token: token) ?? new();
}