////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// LogsServiceTransmission
/// </summary>
public partial class LogsServiceTransmissionMQTT(IMQTTClient mqClient) : ILogsService
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> GoToPageForRowLogsAsync(TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel> req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseStandardModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GoToPageForRowLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TPaginationResponseStandardModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.LogsSelectStorageReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<LogsMetadataResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MetadataLogsReceive, req, token: token) ?? new();
}