////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// InitPaymentMerchantTBankReceive
/// </summary>
public class InitPaymentMerchantTBankReceive(IMerchantService merchantRepo)
    : IResponseReceive<InitMerchantTBankRequestModel?, TResponseModel<PaymentInitTBankResultModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.InitPaymentMerchantTBankReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentInitTBankResultModelDB>?> ResponseHandleActionAsync(InitMerchantTBankRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await merchantRepo.InitPaymentMerchantTBankAsync(req, token);
    }
}