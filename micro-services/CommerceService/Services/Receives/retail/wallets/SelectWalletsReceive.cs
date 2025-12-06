////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectWallets
/// </summary>
public class SelectWalletsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel>?, TPaginationResponseModel<WalletRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectWalletsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectWalletsAsync(req, token);
    }
}