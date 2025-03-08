////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class RegisterJobTempKladrReceive(ILogger<RegisterJobTempKladrReceive> LoggerRepo, IKladrService kladrRepo)
    : IResponseReceive<RegisterJobTempKladrRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.RegisterJobTempKladrReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleAction(RegisterJobTempKladrRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await kladrRepo.RegisterJobTempKladr(req);
    }
}