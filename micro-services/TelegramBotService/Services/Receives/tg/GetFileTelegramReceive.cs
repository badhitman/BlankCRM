////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить файл из Telegram
/// </summary>
public class GetFileTelegramReceive(ITelegramBotService tgRepo)
    : IResponseReceive<string?, TResponseModel<byte[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileTelegramReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>?> ResponseHandleActionAsync(string? fileId, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(fileId);
        return await tgRepo.GetFileTelegramAsync(fileId, token);
    }
}