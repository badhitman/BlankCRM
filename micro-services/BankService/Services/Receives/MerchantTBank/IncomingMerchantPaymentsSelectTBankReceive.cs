////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// IncomingMerchantPaymentsSelectTBankReceive
/// </summary>
public class IncomingMerchantPaymentsSelectTBankReceive(IMerchantService merchantRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>?, TPaginationResponseModel<IncomingMerchantPaymentTBankModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentsSelectTBankReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<IncomingMerchantPaymentTBankModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectIncomingMerchantPaymentsTBankRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await merchantRepo.IncomingMerchantPaymentsSelectTBankAsync(req, token);
    }
}