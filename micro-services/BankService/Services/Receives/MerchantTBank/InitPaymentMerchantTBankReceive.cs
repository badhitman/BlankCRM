////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// InitPaymentMerchantTBankReceive
/// </summary>
public class InitPaymentMerchantTBankReceive(IMerchantService merchantRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<InitMerchantTBankRequestModel?, TResponseModel<PaymentInitTBankResultModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.InitPaymentMerchantTBankReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentInitTBankResultModelDB>?> ResponseHandleActionAsync(InitMerchantTBankRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<PaymentInitTBankResultModelDB> res = await merchantRepo.InitPaymentMerchantTBankAsync(req, token);
        trace.TraceReceiverRecordId = res.Response?.Id.ToString();
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}