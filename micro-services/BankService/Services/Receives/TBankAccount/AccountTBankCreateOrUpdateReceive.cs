////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// TBankAccountCreateOrUpdateReceive
/// </summary>
public class AccountTBankCreateOrUpdateReceive(IBankService bankRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TBankAccountModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AccountTBankCreateOrUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TBankAccountModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id);
        TResponseModel<int> res = await bankRepo.AccountTBankCreateOrUpdateAsync(req, token);
        if (req.Id <= 0)
            trace.TraceReceiverRecordId = res.Response;
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}