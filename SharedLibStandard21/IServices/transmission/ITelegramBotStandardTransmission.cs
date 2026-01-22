////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// ITelegramBotStandardTransmission
/// </summary>
public interface ITelegramBotStandardTransmission : ITelegramBotStandardService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> UserTelegramPermissionUpdateAsync(UserTelegramPermissionSetModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<List<UserTelegramStandardModel>> UsersReadTelegramAsync(int[] req, CancellationToken token = default);
}