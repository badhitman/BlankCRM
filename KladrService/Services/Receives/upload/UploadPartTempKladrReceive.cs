////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class UploadPartTempKladrReceive(ILogger<UploadPartTempKladrReceive> LoggerRepo, IKladrService kladrRepo)
    : IResponseReceive<UploadPartTableDataModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.UploadPartTempKladrReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(UploadPartTableDataModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await kladrRepo.UploadPartTempKladr(req, token);
    }
}