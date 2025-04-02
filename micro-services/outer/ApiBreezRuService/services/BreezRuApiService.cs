////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace ApiBreezRuService;

public class BreezRuApiService(IHttpClientFactory HttpClientFactory, ILogger<BreezRuApiService> logger, IDbContextFactory<ApiBreezRuContext> kladrDbFactory) : IBreezRuApiService
{
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("https://api.breez.ru/v1/leftovers/", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        List<BreezRuGoodsModel>? parseData = JsonConvert.DeserializeObject<List<BreezRuGoodsModel>>(responseBody);

        if (parseData is null)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: parseData is null";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        ApiBreezRuContext ctx = await kladrDbFactory.CreateDbContextAsync(token);
        await ctx.AddRangeAsync(parseData.Select(BreezRuElementModelDB.Build), token);
        await ctx.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}. Записано элементов: {parseData.Count}");
    }
}