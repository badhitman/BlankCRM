////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PriceFullFileGetJson
/// </summary>
public class PriceFullFileGetJsonReceive(ICommerceService commRepo)
    : IResponseReceive<object?, FileAttachModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PriceFullFileGetJsonCommerceReceive;

    /// <inheritdoc/>
    public async Task<FileAttachModel?> ResponseHandleActionAsync(object? req, CancellationToken token = default)
    {
        return await commRepo.PriceFullFileGetJsonAsync(token);
    }
}