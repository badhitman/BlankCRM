////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BankConnectionCreateOrUpdate
/// </summary>
public class BankConnectionCreateOrUpdateReceive(IBankService bankRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<BankConnectionModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BankConnectionCreateOrUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(BankConnectionModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id.ToString());
        TResponseModel<int> res = await bankRepo.BankConnectionCreateOrUpdateAsync(req, token);
        if (req.Id < 1)
            trace.TraceReceiverRecordId = res.Response.ToString();

        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}