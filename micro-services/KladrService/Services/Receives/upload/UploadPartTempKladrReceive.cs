////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class UploadPartTempKladrReceive(IKladrService kladrRepo)
    : IResponseReceive<UploadPartTableDataModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UploadPartTempKladrReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(UploadPartTableDataModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.UploadPartTempKladrAsync(req, token);
    }
}