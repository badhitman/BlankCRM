////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateWallet
/// </summary>
public class UpdateWalletReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<WalletRetailModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateWalletRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(WalletRetailModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.WalletTypeId.ToString());
        ResponseBaseModel res = await commRepo.UpdateWalletAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}