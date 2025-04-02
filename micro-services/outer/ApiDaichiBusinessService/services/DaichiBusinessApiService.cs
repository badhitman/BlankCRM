////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiDaichiBusinessService;

public class DaichiBusinessApiService(IHttpClientFactory HttpClientFactory,
    IOptions<TokenVersionModel> _conf,
    ILogger<DaichiBusinessApiService> logger,
    IDbContextFactory<ApiDaichiBusinessContext> dbFactory) : IDaichiBusinessApiService
{
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"/products/get/?access-token={_conf.Value.Token}", token);
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

        ProductsDaichiBusinessResultModel? parseData = JsonConvert.DeserializeObject<ProductsDaichiBusinessResultModel>(responseBody);

        if (parseData is null)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: parseData is null";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);

        //await ctx.AddRangeAsync(parseData.Select(BreezRuElementModelDB.Build), token);
        //await ctx.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}.");
    }
}