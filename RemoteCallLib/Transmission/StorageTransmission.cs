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
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRow(TPaginationRequestModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NLogRecordModelDB>>(GlobalStaticConstants.TransmissionQueues.GoToPageForRowReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogs(PeriodDatesTimesModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<LogsMetadataResponseModel>>(GlobalStaticConstants.TransmissionQueues.MetadataLogsReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelect(TPaginationRequestModel<LogsSelectRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<NLogRecordModelDB>>(GlobalStaticConstants.TransmissionQueues.LogsSelectStorageReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<StorageFileModelDB>> SaveFile(TAuthRequestModel<StorageImageMetadataModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<StorageFileModelDB>>(GlobalStaticConstants.TransmissionQueues.SaveFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FileContentModel>> ReadFile(TAuthRequestModel<RequestFileReadModel>? req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FileContentModel>>(GlobalStaticConstants.TransmissionQueues.ReadFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelect(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<StorageFileModelDB>>(GlobalStaticConstants.TransmissionQueues.FilesSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadata(FilesAreaMetadataRequestModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<FilesAreaMetadataModel[]>>(GlobalStaticConstants.TransmissionQueues.FilesAreaGetMetadataReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<TagModelDB>> TagsSelect(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<TagModelDB>>(GlobalStaticConstants.TransmissionQueues.TagsSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> TagSet(TagSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstants.TransmissionQueues.TagSetReceive, req, token: token) ?? new();

    #region storage
    /// <inheritdoc/>
    public async Task<TResponseModel<List<T>?>> ReadParameters<T>(StorageMetadataModel[] req, CancellationToken token = default)
    {
        TResponseModel<List<StorageCloudParameterPayloadModel>>? response_payload = await rabbitClient.MqRemoteCallAsync<TResponseModel<List<StorageCloudParameterPayloadModel>>>(GlobalStaticConstants.TransmissionQueues.ReadCloudParametersReceive, req, token: token);
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
    public async Task<TResponseModel<T?>> ReadParameter<T>(StorageMetadataModel req, CancellationToken token = default)
    {
        TResponseModel<StorageCloudParameterPayloadModel>? response_payload = await rabbitClient.MqRemoteCallAsync<TResponseModel<StorageCloudParameterPayloadModel>>(GlobalStaticConstants.TransmissionQueues.ReadCloudParameterReceive, req, token: token);
        TResponseModel<T?> res = new() { Messages = response_payload?.Messages ?? [] };

        if (response_payload?.Success() != true)
            return res;

        if (response_payload.Response is null)
            return res;

        res.Response = JsonConvert.DeserializeObject<T>(response_payload.Response.SerializedDataJson);
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<T?[]?>> FindParameters<T>(RequestStorageBaseModel req, CancellationToken token = default)
    {
        TResponseModel<FoundParameterModel[]>? response_payload = await rabbitClient.MqRemoteCallAsync<TResponseModel<FoundParameterModel[]>>(GlobalStaticConstants.TransmissionQueues.FindCloudParameterReceive, req, token: token);
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
    public async Task<TResponseModel<int>> SaveParameter<T>(T payload_query, StorageMetadataModel store, bool trim, bool waitResponse = true, CancellationToken token = default)
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

        return await rabbitClient.MqRemoteCallAsync<TResponseModel<int>>(GlobalStaticConstants.TransmissionQueues.SaveCloudParameterReceive, set_req, waitResponse, token) ?? new();
    }
    #endregion
}