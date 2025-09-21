////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// CustomersBanksSelectReceive
/// </summary>
public class CustomersBanksSelectReceive(IBankService bankRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel>?, TPaginationResponseModel<CustomerBankIdModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CustomersBanksSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<CustomerBankIdModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectCustomersBanksIdsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await bankRepo.CustomersBanksSelectAsync(req, token);
    }
}