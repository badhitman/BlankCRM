////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// ConnectionsBanksSelectReceive
/// </summary>
public class IncomingTBankMerchantPaymentReceive(IMerchantService merchantRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<JObject?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IncomingTBankMerchantPaymentReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(JObject? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, req.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await merchantRepo.IncomingTBankMerchantPaymentAsync(req, token);
    }
}