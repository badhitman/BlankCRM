////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateWalletType
/// </summary>
public class CreateWalletTypeReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<WalletRetailTypeModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateWalletTypeRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(WalletRetailTypeModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<int> res = await commRepo.CreateWalletTypeAsync(req, token);
        if (res.Response > 0)
        {
            trace.TraceReceiverRecordId = res.Response;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }
        return res;
    }
}