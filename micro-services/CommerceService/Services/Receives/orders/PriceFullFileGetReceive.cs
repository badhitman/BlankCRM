////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PriceFullFileGetReceive
/// </summary>
public class PriceFullFileGetReceive(ICommerceService commRepo) : IResponseReceive<object?, FileAttachModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.PriceFullFileGetCommerceReceive;

    /// <inheritdoc/>
    public async Task<FileAttachModel?> ResponseHandleActionAsync(object? req, CancellationToken token = default)
    {
        return await commRepo.GetFullPriceFileAsync(token);
    }
}
