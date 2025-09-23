////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// GetTBankAccountsReceive
/// </summary>
public class GetTBankAccountsReceive(IBankService bankRepo)
    : IResponseReceive<GetTBankAccountsRequestModel?, TResponseModel<List<TBankAccountModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetTBankConnectionAccountsReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TBankAccountModelDB>>?> ResponseHandleActionAsync(GetTBankAccountsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        // loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await bankRepo.GetTBankAccountsAsync(req, token);
    }
}