﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Net.Http.Json;
using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Send Wappi message
/// </summary>
public class SendWappiMessageReceive(
    ILogger<SendWappiMessageReceive> _logger,
    IHttpClientFactory HttpClientFactory,
    IParametersStorageTransmission StorageTransmissionRepo) : IResponseReceive<EntryAltExtModel?, TResponseModel<SendMessageResponseModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SendWappiMessageReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SendMessageResponseModel?>?> ResponseHandleActionAsync(EntryAltExtModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        TResponseModel<SendMessageResponseModel?> res = new();

        TResponseModel<string?> wappiToken = default!, wappiProfileId = default!;
        TResponseModel<bool?> wappiEnable = default!;

        List<Task> tasks = [Task.Run(async () =>
        {
            wappiToken = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.WappiTokenApi);
        }, token), Task.Run(async () =>
        {
            wappiProfileId = await StorageTransmissionRepo.ReadParameterAsync<string?>(GlobalStaticCloudStorageMetadata.WappiProfileId);
        }, token), Task.Run(async () =>
        {
            wappiEnable = await StorageTransmissionRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ParameterEnabledWappi);
        }, token)];
        await Task.WhenAll(tasks);

        if (wappiEnable.Response != true)
        {
            res.AddInfo("Wappi деактивирован - сообщения не отправляются");
            return res;
        }

        if (!wappiToken.Success() || string.IsNullOrWhiteSpace(wappiToken.Response) || !wappiProfileId.Success() || string.IsNullOrWhiteSpace(wappiProfileId.Response))
        {
            _logger.LogError($"Не удалось отправить сообщение Wappi ({req}): не удалось прочитать настройки");

            res.AddRangeMessages(wappiToken.Messages);
            res.AddRangeMessages(wappiProfileId.Messages);
            res.AddInfo("Wappi не настроен. Активирован, но сообщения не отправляются");

            return res;
        }

        using HttpClient client = HttpClientFactory.CreateClient(HttpClientsNamesEnum.Wappi.ToString());
        if (!client.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
            client.DefaultRequestHeaders.Add("Authorization", wappiToken.Response);

        using HttpResponseMessage response = await client.PostAsJsonAsync($"/api/sync/message/send?profile_id={wappiProfileId.Response}", new SendMessageRequestModel() { Body = req.Text, Recipient = req.Number }, cancellationToken: token);

        if (!response.IsSuccessStatusCode)
        {
            string _msg = $"http err (wappi): {response.StatusCode} ({response.Content.ReadAsStringAsync(token)})\n\n{JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}";
            _logger.LogError(_msg);
            res.AddError(_msg);
        }
        else
        {
            string rj = await response.Content.ReadAsStringAsync(token);
            res.Response = JsonConvert.DeserializeObject<SendMessageResponseModel>(rj);
            res.AddSuccess($"Сообщение успешно отправлено: {res.Response?.Status}");
        }

        return res;
    }
}