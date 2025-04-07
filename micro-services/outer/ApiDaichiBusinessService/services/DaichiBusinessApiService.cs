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
        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);

        TResponseModel<ProductsDaichiBusinessResultModel> data1 = default!;
        TResponseModel<ProductsParamsDaichiBusinessResponseModel> data2 = default!;
        TResponseModel<StoresDaichiBusinessResponseModel> data3 = default!;

        await Task.WhenAll([
            Task.Run(async () => { data1 = await ProductsGetAsync(new ProductsRequestDaichiModel(), token); }, token),
            Task.Run(async () => { data2 = await ProductsParamsGetAsync(new(){ PageSize = 100 }, token); }, token),
            Task.Run(async () => { data3 = await StoresGetAsync(token); }, token)
            ]);

        //await ctx.AddRangeAsync(parseData.Select(BreezRuElementModelDB.Build), token);
        //await ctx.SaveChangesAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}.");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsDaichiBusinessResultModel>> ProductsGetAsync(ProductsRequestDaichiModel req, CancellationToken token = default)
    {
        TResponseModel<ProductsDaichiBusinessResultModel> res = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"/products/get/?access-token={_conf.Value.Token}&store-id={req.StoreId}", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(ProductsGetAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(ProductsGetAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        res.Response = JsonConvert.DeserializeObject<ProductsDaichiBusinessResultModel>(responseBody);

        if (res.Response is null)
        {
            msg = $"Error `{nameof(ProductsGetAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsParamsDaichiBusinessResponseModel>> ProductsParamsGetAsync(ProductParamsRequestDaichiModel req, CancellationToken token = default)
    {
        TResponseModel<ProductsParamsDaichiBusinessResponseModel> res = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"/productparams/get/?access-token={_conf.Value.Token}", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(ProductsParamsGetAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(ProductsParamsGetAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        res.Response = JsonConvert.DeserializeObject<ProductsParamsDaichiBusinessResponseModel>(responseBody);

        if (res.Response is null)
        {
            msg = $"Error `{nameof(ProductsParamsGetAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<StoresDaichiBusinessResponseModel>> StoresGetAsync(CancellationToken token = default)
    {
        TResponseModel<StoresDaichiBusinessResponseModel> res = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"/stores/get/?access-token={_conf.Value.Token}", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(StoresGetAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(StoresGetAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        res.Response = JsonConvert.DeserializeObject<StoresDaichiBusinessResponseModel>(responseBody);

        if (res.Response is null)
        {
            msg = $"Error `{nameof(StoresGetAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }
}