////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// TBankAccountCreateOrUpdateReceive
/// </summary>
public class AccountTBankCreateOrUpdateReceive(IBankService bankRepo, ILogger<AccountTBankCreateOrUpdateReceive> loggerRepo) 
    : IResponseReceive<TBankAccountModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AccountTBankCreateOrUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TBankAccountModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await bankRepo.AccountTBankCreateOrUpdateAsync(req, token);
    }
}