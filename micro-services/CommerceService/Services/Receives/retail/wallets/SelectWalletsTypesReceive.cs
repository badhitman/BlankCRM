////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectWalletsTypes
/// </summary>
public class SelectWalletsTypesReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>?, TPaginationResponseStandardModel<WalletRetailTypeViewModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectWalletsTypesRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WalletRetailTypeViewModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectWalletsTypesAsync(req, token);
    }
}