////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Serialize Storage Remote Transmission Service
/// </summary>
public class StorageTransmission(IMQStandardClientRPC rabbitClient) : IStorageTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> GoToPageForRowLogsAsync(TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GoToPageForRowLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<LogsMetadataResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MetadataLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.LogsSelectStorageReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestStandardModel<StorageFileMetadataModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<StorageFileModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestStandardModel<RequestFileReadModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileContentModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<StorageFileModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FilesSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadataAsync(FilesAreaMetadataRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FilesAreaMetadataModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.FilesAreaGetMetadataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<DirectoryReadResponseModel>> GetDirectoryInfoAsync(DirectoryReadRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<DirectoryReadResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetDirectoryInfoReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<Dictionary<DirectionsEnum, byte[]>>> ReadFileDataAboutPositionAsync(ReadFileDataAboutPositionRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<Dictionary<DirectionsEnum, byte[]>>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileDataAboutPositionReceive, req, token: token) ?? new();
}