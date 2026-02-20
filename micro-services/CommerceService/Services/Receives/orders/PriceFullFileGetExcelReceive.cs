////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PriceFullFileGetExcel
/// </summary>
public class PriceFullFileGetExcelReceive(ICommerceService commRepo)
    : IResponseReceive<object?, FileAttachModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PriceFullFileGetExcelCommerceReceive;

    /// <inheritdoc/>
    public async Task<FileAttachModel?> ResponseHandleActionAsync(object? req, CancellationToken token = default)
    {
        return await commRepo.PriceFullFileGetExcelAsync(token);
    }
}