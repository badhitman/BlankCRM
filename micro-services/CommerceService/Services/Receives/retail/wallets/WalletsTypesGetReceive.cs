////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// WalletsTypesGet
/// </summary>
public class WalletsTypesGetReceive(IRetailService commRepo)
    : IResponseReceive<int[]?, TResponseModel<WalletRetailTypeViewModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WalletsTypesGetRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<WalletRetailTypeViewModel[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.WalletsTypesGetAsync(req, token);
    }
}