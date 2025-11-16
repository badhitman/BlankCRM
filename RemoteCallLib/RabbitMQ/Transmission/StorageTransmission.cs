////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Serialize Storage Remote Transmission Service
/// </summary>
public class StorageTransmission(IRabbitClient rabbitClient) : IStorageTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRowAsync(TPaginationRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GoToPageForRowLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<LogsMetadataResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MetadataLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.LogsSelectStorageReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageFileMetadataModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<StorageFileModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestModel<RequestFileReadModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileContentModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<StorageFileModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FilesSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadataAsync(FilesAreaMetadataRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FilesAreaMetadataModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.FilesAreaGetMetadataReceive, req, token: token) ?? new();
}