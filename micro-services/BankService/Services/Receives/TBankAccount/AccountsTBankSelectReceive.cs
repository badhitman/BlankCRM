////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// TBanksAccountsSelectReceive
/// </summary>
public class AccountsTBankSelectReceive(IBankService bankRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<SelectAccountsRequestModel>?, TPaginationResponseModel<TBankAccountModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AccountsTBankSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TBankAccountModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectAccountsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await bankRepo.AccountsTBankSelectAsync(req, token);
    }
}