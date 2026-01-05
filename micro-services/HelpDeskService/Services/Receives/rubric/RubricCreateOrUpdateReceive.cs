////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// CreateIssueTheme
/// </summary>
public class RubricCreateOrUpdateReceive(IRubricsService hdRepo, ILogger<RubricCreateOrUpdateReceive> loggerRepo)
    : IResponseReceive<RubricStandardModel?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(RubricStandardModel? rubric, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rubric);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(rubric, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await hdRepo.RubricCreateOrUpdateAsync(rubric, token);
    }
}