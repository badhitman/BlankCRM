////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// IncomingMerchantPaymentsSelectTBank
/// </summary>
public class IncomingMerchantPaymentsSelectTBankReceive(IMerchantService merchantRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>?, TPaginationResponseStandardModel<IncomingMerchantPaymentTBankModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentsSelectTBankReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<IncomingMerchantPaymentTBankModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await merchantRepo.IncomingMerchantPaymentsSelectTBankAsync(req, token);
    }
}