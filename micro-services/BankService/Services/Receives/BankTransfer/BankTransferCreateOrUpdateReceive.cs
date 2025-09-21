////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BankTransferCreateOrUpdateReceive
/// </summary>
public class BankTransferCreateOrUpdateReceive(IBankService bankRepo, ILogger<BankTransferCreateOrUpdateReceive> loggerRepo) 
    : IResponseReceive<BankTransferModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BankTransferCreateOrUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(BankTransferModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await bankRepo.BankTransferCreateOrUpdateAsync(req, token);
    }
}