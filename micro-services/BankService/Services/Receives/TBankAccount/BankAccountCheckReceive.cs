////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BankAccountCheckReceive
/// </summary>
public class BankAccountCheckReceive(IBankService bankRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<BankAccountCheckRequestModel?, TResponseModel<List<BankTransferModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BankAccountCheckReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BankTransferModelDB>>?> ResponseHandleActionAsync(BankAccountCheckRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, req.ToString(), req.BankConnectionId);
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await bankRepo.BankAccountCheckAsync(req, token);
    }
}