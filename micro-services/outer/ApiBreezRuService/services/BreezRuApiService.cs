////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace ApiBreezRuService;

/// <summary>
/// BreezRuApiService
/// </summary>
public class BreezRuApiService(IHttpClientFactory HttpClientFactory, ILogger<BreezRuApiService> logger, IDbContextFactory<ApiBreezRuContext> dbFactory) : IBreezRuApiService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("/leftovers/", token);
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

        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        await ctx.AddRangeAsync(parseData.Select(BreezRuElementModelDB.Build), token);
        await ctx.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}. Записано элементов: {parseData.Count}");
    }
}