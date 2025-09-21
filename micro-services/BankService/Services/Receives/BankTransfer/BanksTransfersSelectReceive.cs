////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.bank;

/// <summary>
/// BanksTransfersSelectReceive
/// </summary>
public class BanksTransfersSelectReceive(IBankService bankRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>?, TPaginationResponseModel<BankTransferModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.BanksTransfersSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<BankTransferModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectTransfersBanksRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await bankRepo.BanksTransfersSelectAsync(req, token);
    }
}