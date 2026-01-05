////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Сдвинуть рубрику
/// </summary>
public class RubricMoveReceive(IRubricsService hdRepo, ILogger<RubricMoveReceive> loggerRepo)
    : IResponseReceive<TAuthRequestModel<RowMoveModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesMoveHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestModel<RowMoveModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await hdRepo.RubricMoveAsync(req, token);
    }
}