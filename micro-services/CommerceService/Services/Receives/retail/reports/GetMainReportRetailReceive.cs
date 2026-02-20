////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// GetMainReport
/// </summary>
public class GetMainReportRetailReceive(IRetailService commRepo)
    : IResponseReceive<MainReportRequestModel?, MainReportResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetMainReportRetailReceive;

    /// <inheritdoc/>
    public async Task<MainReportResponseModel?> ResponseHandleActionAsync(MainReportRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetMainReportAsync(req, token);
    }
}