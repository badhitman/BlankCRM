////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BankTransferCreateOrUpdateReceive
/// </summary>
public class BankTransferCreateOrUpdateReceive(IBankService bankRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<BankTransferModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BankTransferCreateOrUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(BankTransferModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id);
        TResponseModel<int> res = await bankRepo.BankTransferCreateOrUpdateAsync(req, token);
        if (trace.TraceReceiverRecordId is null || trace.TraceReceiverRecordId == 0)
            trace.TraceReceiverRecordId = res.Response;
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}