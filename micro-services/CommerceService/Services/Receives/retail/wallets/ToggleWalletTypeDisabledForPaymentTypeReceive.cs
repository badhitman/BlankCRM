////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// ToggleWalletTypeDisabledForPaymentType
/// </summary>
public class ToggleWalletTypeDisabledForPaymentTypeReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ToggleWalletTypeDisabledForPaymentTypeRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ToggleWalletTypeDisabledForPaymentTypeRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ToggleWalletTypeDisabledForPaymentTypeRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.ToggleWalletTypeDisabledForPaymentTypeAsync(req, token);
    }
}