////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstants;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using ToolsMauiLib;
using static SharedLib.GlobalStaticConstantsRoutes;

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
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> GoToPageForRowLogsAsync(TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel> req, CancellationToken cancellationToken = default)
    {
        using HttpClient _client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _client.PostAsync(new Uri($"/{_conf.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.PAGE_CONTROLLER_NAME}/{Routes.GOTO_ACTION_NAME}-for-{Routes.RECORD_CONTROLLER_NAME}"), content, cancellationToken);

        string rj = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TPaginationResponseStandardModel<NLogRecordModelDB>>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken cancellationToken = default)
    {
        using HttpClient _client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _client.PostAsync(new Uri($"{_conf.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.SELECT_ACTION_NAME}"), content, cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<TPaginationResponseStandardModel<NLogRecordModelDB>>(rj)!;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken cancellationToken = default)
    {
        using HttpClient _client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Kladr.ToString());

        string json = System.Text.Json.JsonSerializer.Serialize(req, _serializerOptions);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _client.PostAsync(new Uri($"{_conf.AddressBaseUri.NormalizedUriEnd()}{Routes.API_CONTROLLER_NAME}/{Routes.TOOLS_CONTROLLER_NAME}/{Routes.LOGS_ACTION_NAME}-{Routes.METADATA_CONTROLLER_NAME}"), content, cancellationToken);
        string rj = await response.Content.ReadAsStringAsync(cancellationToken);

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