////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;
using SharedLib;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace RemoteCallLib;

/// <summary>
/// ParametersStorageTransmission
/// </summary>
public class ParametersStorageTransmission(IMQTTClient rabbitClient) : IParametersStorageTransmission
{
    #region tag`s
    /// <inheritdoc/> tags
    public async Task<TPaginationResponseModel<TagViewModel>> TagsSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<TagViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.TagsSelectReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>> TagSetAsync(TagSetModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<bool>>(GlobalStaticConstantsTransmission.TransmissionQueues.TagSetReceive, req, token: token) ?? new();
    #endregion

    #region parameter`s
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
 