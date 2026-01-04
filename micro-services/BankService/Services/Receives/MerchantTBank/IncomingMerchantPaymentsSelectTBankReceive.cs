////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// IncomingMerchantPaymentsSelectTBankReceive
/// </summary>
public class IncomingMerchantPaymentsSelectTBankReceive(IMerchantService merchantRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>?, TPaginationResponseModel<IncomingMerchantPaymentTBankModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentsSelectTBankReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IncomingMerchantPaymentTBankModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, req.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await merchantRepo.IncomingMerchantPaymentsSelectTBankAsync(req, token);
    }
}