////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить файл из Telegram
/// </summary>
public class GetFileTelegramReceive(ITelegramBotService tgRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, TResponseModel<byte[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileTelegramReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>?> ResponseHandleActionAsync(string? fileId, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(fileId);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, fileId.GetType().Name, JsonConvert.SerializeObject(fileId));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);
        TResponseModel<byte[]> res = await tgRepo.GetFileTelegramAsync(fileId, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}