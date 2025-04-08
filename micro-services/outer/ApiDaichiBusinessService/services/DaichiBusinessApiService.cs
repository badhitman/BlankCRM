////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
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

        TResponseModel<ProductsDaichiBusinessResultModel> products = default!;
        TResponseModel<StoresDaichiBusinessResponseModel> stores = default!;
        List<ParameterElementDaichiJsonModel> parametersAll = [];
        await Task.WhenAll([
            Task.Run(async () =>
            {
                ProductParamsRequestDaichiModel _rq = new (){ PageSize = 100, Page = 1 };
                TResponseModel<ProductsParamsDaichiBusinessResponseModel> data2 = await ProductsParamsGetAsync(_rq, token);
                try
                {
                    while(data2.Response?.Result?.Data is not null && data2.Response.Result.Data.Count != 0)
                    {
                        parametersAll.AddRange( data2.Response.Result.Data.Values.Select(x =>
                        {
                            ParameterElementDaichiJsonModel res = x.ToObject<ParameterElementDaichiJsonModel>()!;
                            JProperty[] _jpts = x.Properties().Where(x => x.Name.StartsWith("attr_", StringComparison.OrdinalIgnoreCase)).ToArray();
                            res.Attributes = [.. _jpts.Select(x => x.First().ToObject<AttributeParameterDaichiModel>())];
                            return res;
                        }));
                        _rq.Page++;
                        data2 = await ProductsParamsGetAsync(_rq, token);
                    }
                }
                catch(Exception ex){
                    logger.LogError(ex, "Ошибка обработки товаров Daichi");
                }

            }, token),
            Task.Run(async () => { products = await ProductsGetAsync(new ProductsRequestDaichiModel(), token); }, token),
            Task.Run(async () => { stores = await StoresGetAsync(token); }, token)
            ]);

        if (stores.Response?.Result is null)
            return ResponseBaseModel.CreateError("stores.Response?.Result is null");

        if (products.Response?.GetProducts is null)
            return ResponseBaseModel.CreateError("products.Response?.GetProducts");

        await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

        await ctx.Stores.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Products.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Attributes.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.AvailabilityGoods.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.PricesProducts.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.ParametersCatalog.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.ParamsProducts.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.SectionsParameters.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.PhotosParameters.ExecuteDeleteAsync(cancellationToken: token);

        List<StoreDaichiModelDB> storesDb = [.. stores.Response.Result.Select(StoreDaichiModelDB.Build)];
        await ctx.AddRangeAsync(storesDb, token);
        await ctx.SaveChangesAsync(token);

        List<ProductDaichiModelDB> productsDb = [.. products.Response.GetProducts.Select(x => ProductDaichiModelDB.Build(x, storesDb))];
        await ctx.AddRangeAsync(productsDb, token);
        await ctx.SaveChangesAsync(token);

        try
        {
            List<ParameterEntryDaichiModelDB> productsParametersDb = [.. parametersAll.Select(x => ParameterEntryDaichiModelDB.Build(x, productsDb))];
            await ctx.AddRangeAsync(productsParametersDb, token);
            await ctx.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка загрузки параметров Daichi");
        }

        await transaction.CommitAsync(token);

        if (products.Response.Exceptions != null && products.Response.Exceptions.Count != 0)
        {
            ResponseBaseModel _r = ResponseBaseModel.CreateSuccess($"Задание выполнено (но с ошибками для {string.Join(",", products.Response.Exceptions.Select(z => z.Key))}): {nameof(DownloadAndSaveAsync)}.");

            foreach (KeyValuePair<string, Exception> ex in products.Response.Exceptions)
            {
                _r.Messages.InjectException(ex.Value);
                logger.LogError(ex.Value, $"Ошибка обработки товара: {ex.Key}");
            }

            return _r;
        }
        else
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
        HttpResponseMessage response = await httpClient.GetAsync($"/productparams/get/?access-token={_conf.Value.Token}&page-size={req.PageSize}&page={req.Page}", token);
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

        try
        {
            res.Response = JsonConvert.DeserializeObject<ProductsParamsDaichiBusinessResponseModel>(responseBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, $"Ошибка десериализации:\n{responseBody}");
        }

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