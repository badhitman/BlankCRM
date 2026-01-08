////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BanksTransfersSelect
/// </summary>
public class BanksTransfersSelectReceive(IBankService bankRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>?, TPaginationResponseStandardModel<BankTransferModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BanksTransfersSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<BankTransferModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await bankRepo.BanksTransfersSelectAsync(req, token);
    }
}