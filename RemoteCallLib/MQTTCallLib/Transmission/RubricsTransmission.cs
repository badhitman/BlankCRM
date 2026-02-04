////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;
using System.Collections.Generic;

namespace RemoteCallLib;

/// <inheritdoc/>
public class RubricsTransmission(IMQTTClient rabbitClient) : IRubricsTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(IEnumerable<int> rubricsIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricStandardModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsForIssuesGetHelpDeskReceive, rubricsIds, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricStandardModel>>> RubricReadWithParentsHierarchyAsync(int rubricId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricStandardModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesReadHelpDeskReceive, rubricId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> RubricCreateOrUpdateAsync(RubricStandardModel issueTheme, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesUpdateReceive, issueTheme, token: token) ?? new();
   
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> RubricMoveAsync(TAuthRequestStandardModel<RowMoveModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricForIssuesMoveHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<UniversalBaseModel>> RubricsChildListAsync(RubricsListRequestStandardModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<UniversalBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsChildListHelpDeskReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RubricStandardModel>>> RubricsGetAsync(int[] req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<List<RubricStandardModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.RubricsChildListHelpDeskReceive, req, token: token) ?? new();
}