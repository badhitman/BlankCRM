////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Serialize Storage Remote Transmission Service
/// </summary>
public class StorageTransmission(IRabbitClient rabbitClient) : IStorageTransmission
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRowAsync(TPaginationRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.GoToPageForRowReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<LogsMetadataResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MetadataLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NLogRecordModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.LogsSelectStorageReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageImageMetadataModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<StorageFileModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestModel<RequestFileReadModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileContentModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<StorageFileModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.FilesSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadataAsync(FilesAreaMetadataRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FilesAreaMetadataModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.FilesAreaGetMetadataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TagModelDB>> TagsSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<TagModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.TagsSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> TagSetAsync(TagSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.TagSetReceive, req, token: token) ?? new();

    #region storage
    /// <inheritdoc/>
    public async Task<TResponseModel<List<T>?>> ReadParametersAsync<T>(StorageMetadataModel[] req, CancellationToken token = default)
    {
        TResponseModel<List<StorageCloudParameterPayloadModel>>? response_payload = await rabbitClient.MqRemoteCallAsync<TResponseModel<List<StorageCloudParameterPayloadModel>>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadCloudParametersReceive, req, token: token);
        TResponseModel<List<T>?> res = new();
        if (response_payload?.Success() != true)
        {
            res.Messages = response_payload?.Messages ?? [];
            return res;
        }

        if (response_payload.Response is null || response_payload.Response.Count == 0)
            return res;

        res.Response = response_payload.Response.Select(x => JsonConvert.DeserializeObject<T>(x.SerializedDataJson)).ToList()!;
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<T?>> ReadParameterAsync<T>(StorageMetadataModel req, CancellationToken token = default)
    {
        TResponseModel<StorageCloudParameterPayloadModel>? response_payload = await rabbitClient.MqRemoteCallAsync<TResponseModel<StorageCloudParameterPayloadModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadCloudParameterReceive, req, token: token);
        TResponseModel<T?> res = new() { Messages = response_payload?.Messages ?? [] };

        if (response_payload?.Success() != true)
            return res;

        if (response_payload.Response is null)
            return res;

        res.Response = JsonConvert.DeserializeObject<T>(response_payload.Response.SerializedDataJson);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<T?[]?>> FindParametersAsync<T>(RequestStorageBaseModel req, CancellationToken token = default)
    {
        TResponseModel<FoundParameterModel[]>? response_payload = await rabbitClient.MqRemoteCallAsync<TResponseModel<FoundParameterModel[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.FindCloudParameterReceive, req, token: token);
        TResponseModel<T?[]?> res = new();
        if (response_payload?.Success() != true)
        {
            res.Messages = response_payload?.Messages ?? [];
            return res;
        }

        if (response_payload.Response is null)
            return res;

        res.Response = [.. response_payload
            .Response
            .Select(x => JsonConvert.DeserializeObject<T>(x.SerializedDataJson))];

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> SaveParameterAsync<T>(T payload_query, StorageMetadataModel store, bool trim, bool waitResponse = true, CancellationToken token = default)
    {
        if (payload_query is null)
            throw new ArgumentNullException(nameof(payload_query));

        StorageCloudParameterPayloadModel set_req = new()
        {
            TrimHistory = trim,
            ApplicationName = store.ApplicationName,
            PropertyName = store.PropertyName,
            SerializedDataJson = JsonConvert.SerializeObject(payload_query, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings),
            TypeName = payload_query.GetType().FullName!,
            OwnerPrimaryKey = store.OwnerPrimaryKey,
            PrefixPropertyName = store.PrefixPropertyName,
        };

        return await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstantsTransmission.TransmissionQueues.SaveCloudParameterReceive, set_req, waitResponse, token) ?? new();
    }
    #endregion
}