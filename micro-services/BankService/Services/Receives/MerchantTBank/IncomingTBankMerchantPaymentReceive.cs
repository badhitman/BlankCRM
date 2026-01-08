////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// IncomingTBankMerchantPayment
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
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await merchantRepo.IncomingTBankMerchantPaymentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}