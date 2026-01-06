////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BankAccountCheckReceive
/// </summary>
public class BankAccountCheckReceive(IBankService bankRepo)
    : IResponseReceive<BankAccountCheckRequestModel?, TResponseModel<List<BankTransferModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BankAccountCheckReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BankTransferModelDB>>?> ResponseHandleActionAsync(BankAccountCheckRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await bankRepo.BankAccountCheckAsync(req, token);
    }
}