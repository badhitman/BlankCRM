////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Установить пользователю Claim`s[TelegramId, FirstName, LastName, PhoneNum]
/// </summary>
public class ClaimsUserFlushReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ClaimsForUserFlushReceive;

    /// <summary>
    /// Установить пользователю Claim`s[TelegramId, FirstName, LastName, PhoneNum]
    /// </summary>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req))
            throw new ArgumentNullException(nameof(req));
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);

        return await idRepo.ClaimsUserFlushAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */