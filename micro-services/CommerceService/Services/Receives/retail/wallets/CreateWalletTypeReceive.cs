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
    : IResponseReceive<TAuthRequestStandardModel<WalletRetailTypeModelDB>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateWalletTypeRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestStandardModel<WalletRetailTypeModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<int> res = await commRepo.CreateWalletTypeAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}