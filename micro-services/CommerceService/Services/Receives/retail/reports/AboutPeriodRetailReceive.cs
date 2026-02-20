////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AboutPeriod
/// </summary>
public class AboutPeriodRetailReceive(IRetailService commRepo)
    : IResponseReceive<object?, PeriodBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AboutPeriodRetailReceive;

    /// <inheritdoc/>
    public async Task<PeriodBaseModel?> ResponseHandleActionAsync(object? req, CancellationToken token = default)
    {
        // ArgumentNullException.ThrowIfNull(req);
        return await commRepo.AboutPeriodAsync(req, token);
    }
}