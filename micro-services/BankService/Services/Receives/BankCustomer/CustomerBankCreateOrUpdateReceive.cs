////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// CustomerBankCreateOrUpdate
/// </summary>
public class CustomerBankCreateOrUpdateReceive(IBankService bankRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<CustomerBankIdModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CustomerBankCreateOrUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(CustomerBankIdModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id.ToString());
        TResponseModel<int> res = await bankRepo.CustomerBankCreateOrUpdateAsync(req, token);
        if (req.Id < 1)
            trace.TraceReceiverRecordId = res.Response.ToString();

        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}