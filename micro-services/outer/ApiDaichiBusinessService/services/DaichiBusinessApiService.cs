////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace ApiDaichiBusinessService;

/// <summary>
/// DaichiBusinessApiService
/// </summary>
public class DaichiBusinessApiService(IHttpClientFactory HttpClientFactory,
    IOptions<TokenVersionModel> _conf,
    ILogger<DaichiBusinessApiService> logger,
    IDbContextFactory<ApiDaichiBusinessContext> dbFactory) : IDaichiBusinessApiService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString());
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

        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);

        //await ctx.AddRangeAsync(parseData.Select(BreezRuElementModelDB.Build), token);
        //await ctx.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}.");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsDaichiBusinessResultModel>> ProductsGetAsync(CancellationToken token = default)
    {
        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel>> ProductsParamsGetAsync(ProductParamsRequestDaichiModel req, CancellationToken token = default)
    {
        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<StoresDaichiBusinessResponseModel>> StoresGetAsync(CancellationToken token = default)
    {
        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
    }
}