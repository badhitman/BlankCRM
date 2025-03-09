﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstants;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using ToolsMauiLib;

namespace ToolsMauiApp;

/// <summary>
/// LogsService
/// </summary>
#pragma warning disable CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
public class LogsService(ApiRestConfigModelDB _conf, IHttpClientFactory HttpClientFactory) : ILogsService
#pragma warning restore CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
{
    JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRow(TPaginationRequestModel<int> req)
    {
        using HttpClient _client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _client.PostAsync(new Uri($"/{_conf.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.PAGE_ACTION_NAME}/{Routes.GOTO_ACTION_NAME}-for-{Routes.RECORD_CONTROLLER_NAME}"), content);

        string rj = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TPaginationResponseModel<NLogRecordModelDB>>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelect(TPaginationRequestModel<LogsSelectRequestModel> req)
    {
        using HttpClient _client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _client.PostAsync(new Uri($"{_conf.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.SELECT_ACTION_NAME}"), content);
        string rj = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TPaginationResponseModel<NLogRecordModelDB>>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogs(PeriodDatesTimesModel req)
    {
        using HttpClient _client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _client.PostAsync(new Uri($"{_conf.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.METADATA_CONTROLLER_NAME}"), content);
        string rj = await response.Content.ReadAsStringAsync();

        try
        {
            return JsonConvert.DeserializeObject<TResponseModel<LogsMetadataResponseModel>>(rj)!;
        }
        catch (Exception ex)
        {
            TResponseModel<LogsMetadataResponseModel> res = new();

            res.Messages.InjectException(ex);
            return res;
        }
    }
}