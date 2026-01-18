////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// IncomingMerchantPaymentTBankReceive
/// </summary>
public class IncomingMerchantPaymentTBankReceive(ICommerceService commerceRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<IncomingMerchantPaymentTBankNotifyModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentTBankReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IncomingMerchantPaymentTBankNotifyModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, req);
        ResponseBaseModel res = await commerceRepo.IncomingMerchantPaymentTBankAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}