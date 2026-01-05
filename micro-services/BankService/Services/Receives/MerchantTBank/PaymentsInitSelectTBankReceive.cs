////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// PaymentsInitSelectTBankReceive
/// </summary>
public class PaymentsInitSelectTBankReceive(IMerchantService merchantRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectInitPaymentsTBankRequestModel>?, TPaginationResponseStandardModel<PaymentInitTBankResultModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PaymentsInitSelectTBankReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<PaymentInitTBankResultModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectInitPaymentsTBankRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await merchantRepo.PaymentsInitSelectTBankAsync(req, token);
    }
}