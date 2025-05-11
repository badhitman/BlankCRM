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
public class RubricCreateOrUpdateReceive(IHelpDeskService hdRepo, ILogger<RubricCreateOrUpdateReceive> loggerRepo) : IResponseReceive<RubricIssueHelpDeskModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(RubricIssueHelpDeskModelDB? rubric, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(rubric);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(rubric)}");
        return await hdRepo.RubricCreateOrUpdateAsync(rubric, token);
    }
}