////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// InitPaymentMerchantTBankReceive
/// </summary>
public class InitPaymentMerchantTBankReceive(IMerchantService merchantRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<InitMerchantTBankRequestModel>?, TResponseModel<PaymentInitTBankResultModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.InitPaymentMerchantTBankReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentInitTBankResultModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<InitMerchantTBankRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<PaymentInitTBankResultModelDB> res = await merchantRepo.InitPaymentMerchantTBankAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}