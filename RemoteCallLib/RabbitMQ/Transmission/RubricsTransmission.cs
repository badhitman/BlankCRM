////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class RubricsTransmission(IRabbitClient rabbitClient) : IRubricsTransmission
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TelegramMessageIncomingAsync(TelegramIncomingMessageModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.IncomingTelegramMessageHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpDeskModelDB>>> RubricsGetAsync(IEnumerable<int> rubricsIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpDeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesGetHelpDeskReceive, rubricsIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricIssueHelpDeskModelDB>>> RubricReadAsync(int rubricId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricIssueHelpDeskModelDB>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesReadHelpDeskReceive, rubricId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricIssueHelpDeskModelDB issueTheme, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateReceive, issueTheme, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> RubricsListAsync(RubricsListRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<UniversalBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesListHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestModel<RowMoveModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesMoveHelpDeskReceive, req, token: token) ?? new();
}