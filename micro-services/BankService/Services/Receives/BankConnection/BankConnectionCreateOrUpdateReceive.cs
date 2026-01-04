////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BankConnectionCreateOrUpdateReceive
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

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, req.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await bankRepo.BankConnectionCreateOrUpdateAsync(req, token);
    }
}