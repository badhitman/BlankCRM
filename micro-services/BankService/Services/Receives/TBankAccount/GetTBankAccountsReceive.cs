////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// GetTBankAccountsReceive
/// </summary>
public class GetTBankAccountsReceive(IBankService bankRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<GetTBankAccountsRequestModel?, TResponseModel<List<TBankAccountModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetTBankConnectionAccountsReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TBankAccountModelDB>>?> ResponseHandleActionAsync(GetTBankAccountsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, req.ToString(), req.BankConnectionId);
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await bankRepo.GetTBankAccountsAsync(req, token);
    }
}