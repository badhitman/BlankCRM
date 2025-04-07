////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;
using System.Net.Http.Json;

namespace ApiRusklimatComService;

/// <summary>
/// RusklimatComApiService
/// </summary>
public class RusklimatComApiService(
    IHttpClientFactory HttpClientFactory,
    IMemoryCache memoryCache,
    IOptions<AuthAlterModel> _conf,
    ILogger<RusklimatComApiService> logger,
    IDbContextFactory<ApiRusklimatComContext> dbFactory) : IRusklimatComApiService, IDisposable
{
    HttpClient? httpClient;
    string? requestKey;
    const string _jwt = "rusklimat-jwt", _requestKeyCache = "rusklimat-RequestKey", _pref = "InternetPartner";

    async Task GetClient(bool perm = false, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(_conf.Value.UserId) || string.IsNullOrWhiteSpace(_conf.Value.Password))
        {
            logger.LogError($"string.IsNullOrWhiteSpace(_conf.Value.UserId) || string.IsNullOrWhiteSpace(_conf.Value.Password)");
            httpClient = null;
            return;
        }
        // !memoryCache.TryGetValue(_jwt, out string? jwt) || string.IsNullOrWhiteSpace(jwt)
        if (perm || !memoryCache.TryGetValue(_requestKeyCache, out requestKey) || string.IsNullOrWhiteSpace(requestKey))
            httpClient = null;
        else if (httpClient is not null)
            return;

        if (!memoryCache.TryGetValue(_jwt, out string? jwt) || string.IsNullOrWhiteSpace(jwt))
        {
            using HttpClient _httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.AuthRusklimatComJWT.ToString());
            Dictionary<string, string> dict = new()
            {
                { "login", _conf.Value.UserId },
                { "password", _conf.Value.Password },
            };

            FormUrlEncodedContent content = new(dict);
            HttpResponseMessage response;
            int i = 0;
            string responseBody;
            do
            {
                if (i != 0)
                    logger.LogWarning($"+ trying ({i}) auth RUSKLIMAT");

                response = await _httpClient.PostAsJsonAsync($"{_conf.Value.Version}/auth/jwt/", dict, token);
                responseBody = await response.Content.ReadAsStringAsync(token);
                if (!response.IsSuccessStatusCode)
                    await Task.Delay(200, token);
                else
                    break;
            } while (!response.IsSuccessStatusCode && ++i < 3);

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseBody))
            {
                logger.LogError($"rusklimat http-client auth error: {responseBody}");
                return;
            }

            Dictionary<string, object>? authorizeData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            if (authorizeData is null || authorizeData.Count == 0 || int.Parse(authorizeData["code"].ToString()!) != 200)
            {
                logger.LogError($"authorizeData is null || authorizeData.Count == 0");
                return;
            }

            jwt = authorizeData["data"].ToString();
            if (string.IsNullOrWhiteSpace(jwt))
            {
                logger.LogError($"string.IsNullOrWhiteSpace(jwt)");
                return;
            }
            authorizeData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jwt);
            if (authorizeData is null || authorizeData.Count == 0)
            {
                logger.LogError($"authorizeData is null || authorizeData.Count == 0");
                return;
            }
            jwt = authorizeData["jwtToken"].ToString();
            if (string.IsNullOrWhiteSpace(jwt))
            {
                logger.LogError($"string.IsNullOrWhiteSpace(jwt)");
                return;
            }
            //
            memoryCache.Set(_jwt, jwt, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiRusklimatCom.ToString());
        httpClient.DefaultRequestHeaders.Add("Authorization", jwt);
        using HttpResponseMessage _keyRest = await httpClient.GetAsync($"{_conf.Value.Version}/{_pref}/{_conf.Value.PartnerId}/requestKey/", token);
        string _respString = await _keyRest.Content.ReadAsStringAsync(token);
        if (!_keyRest.IsSuccessStatusCode)
        {
            logger.LogError($"requestKey error (code:{_keyRest.StatusCode}) - {_respString}");
            httpClient = null;
            return;
        }
        if (string.IsNullOrWhiteSpace(_respString))
        {
            logger.LogError($"string.IsNullOrWhiteSpace(_respString)");
            httpClient = null;
            return;
        }
        RequestKeyModel? _rk = JsonConvert.DeserializeObject<RequestKeyModel>(_respString);
        if (string.IsNullOrWhiteSpace(_rk?.RequestKey))
        {
            logger.LogError($"string.IsNullOrWhiteSpace(_rk?.RequestKey)");
            httpClient = null;
            return;
        }
        requestKey = _rk.RequestKey;
        memoryCache.Set(_requestKeyCache, _rk.RequestKey, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
    }


    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        TResponseModel<UnitsRusklimatResponseModel> getUnits = default!;
        TResponseModel<CategoriesRusklimatResponseModel> getCats = default!;
        TResponseModel<PropertiesRusklimatResponseModel> getProps = default!;

        List<ProductRusklimatModel> prodsData = [];

        await Task.WhenAll(
            Task.Run(async () =>
            {
                RusklimatPaginationRequestModel _req = new() { PageNum = 1, PageSize = 1000 };
                TResponseModel<ProductsRusklimatResponseModel> getProds = await GetProductsAsync(_req, token);

                try
                {
                    while (getProds.Response?.Data is not null && getProds.Response.Data.Length != 0)
                    {
                        prodsData.AddRange(getProds.Response.Data);

                        _req.PageNum++;
                        getProds = await GetProductsAsync(_req, token);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка обработки товаров Rusklimat");
                }

            }, token),
            Task.Run(async () => { getUnits = await GetUnitsAsync(token); }, token),
            Task.Run(async () => { getCats = await GetCategoriesAsync(token); }, token),
            Task.Run(async () => { getProps = await GetPropertiesAsync(token); }, token)
        );

        if (getUnits.Response?.Data is null)
            return ResponseBaseModel.CreateError("getUnits.Response?.Data is null");

        if (getCats.Response?.Data is null)
            return ResponseBaseModel.CreateError("getCats.Response is null");

        if (getProps.Response?.Data is null)
            return ResponseBaseModel.CreateError("getProps.Response is null");

        if (prodsData.Count == 0)
            return ResponseBaseModel.CreateError("prodsData.Count == 0");

        using ApiRusklimatComContext ctx = await dbFactory.CreateDbContextAsync(token);
        await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

        await ctx.Units.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Categories.ExecuteDeleteAsync(cancellationToken: token);

        await ctx.ProductsInformation.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.ProductsProperties.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.PropertiesCatalog.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Remains.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.WarehousesRemains.ExecuteDeleteAsync(cancellationToken: token);

        await ctx.Products.ExecuteDeleteAsync(cancellationToken: token);

        await ctx.AddRangeAsync(getUnits.Response.Data, token);
        await ctx.SaveChangesAsync(token);

        await ctx.AddRangeAsync(getCats.Response.Data, token);
        await ctx.SaveChangesAsync(token);

        await ctx.AddRangeAsync(getProps.Response.Data, token);
        await ctx.SaveChangesAsync(token);

        try
        {
            List<ProductRusklimatModelDB> productsDb = [.. prodsData.Select(x => ProductRusklimatModelDB.Build(x, getProps.Response.Data))];
            await ctx.AddRangeAsync(productsDb, token);
            await ctx.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка загрузки товаров");
        }

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess($"Обработано");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsRusklimatResponseModel>> GetProductsAsync(RusklimatPaginationRequestModel req, CancellationToken token = default)
    {
        TResponseModel<ProductsRusklimatResponseModel> res = new();
        await GetClient(token: token);
        if (httpClient is null)
        {
            res.AddError("httpClient is null");
            return res;
        }

        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"v2/{_pref}/{_conf.Value.PartnerId}/products/{requestKey}/?pageSize={req.PageSize}&page={req.PageNum}", req, token);
        string msg;
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetProductsAsync)}` (http code: {response.StatusCode}): {responseBody}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetProductsAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        try
        {
            res.Response = JsonConvert.DeserializeObject<ProductsRusklimatResponseModel>(responseBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Ошибка десериализации");
        }

        if (res.Response is null)
        {
            msg = $"Error `{nameof(GetProductsAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<CategoriesRusklimatResponseModel>> GetCategoriesAsync(CancellationToken token = default)
    {
        TResponseModel<CategoriesRusklimatResponseModel> res = new();
        await GetClient(token: token);
        if (httpClient is null)
        {
            res.AddError("httpClient is null");
            return res;
        }


        HttpResponseMessage response = await httpClient.GetAsync($"{_conf.Value.Version}/{_pref}/categories/{requestKey}", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetCategoriesAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetCategoriesAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        try
        {
            res.Response = JsonConvert.DeserializeObject<CategoriesRusklimatResponseModel>(responseBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Ошибка десериализации");
        }

        if (res.Response is null)
        {
            msg = $"Error `{nameof(GetCategoriesAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<UnitsRusklimatResponseModel>> GetUnitsAsync(CancellationToken token = default)
    {
        TResponseModel<UnitsRusklimatResponseModel> res = new();
        await GetClient(token: token);
        if (httpClient is null)
        {
            res.AddError("httpClient is null");
            return res;
        }

        HttpResponseMessage response = await httpClient.GetAsync($"{_conf.Value.Version}/{_pref}/units", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetUnitsAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetUnitsAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        try
        {
            res.Response = JsonConvert.DeserializeObject<UnitsRusklimatResponseModel>(responseBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Ошибка десериализации");
        }

        if (res.Response is null)
        {
            msg = $"Error `{nameof(GetUnitsAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PropertiesRusklimatResponseModel>> GetPropertiesAsync(CancellationToken token = default)
    {
        TResponseModel<PropertiesRusklimatResponseModel> res = new();
        await GetClient(token: token);
        if (httpClient is null)
        {
            res.AddError("httpClient is null");
            return res;
        }

        HttpResponseMessage response = await httpClient.GetAsync($"{_conf.Value.Version}/{_pref}/properties/{requestKey}", token);
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetPropertiesAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetPropertiesAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            res.AddError(msg);
            return res;
        }

        try
        {
            res.Response = JsonConvert.DeserializeObject<PropertiesRusklimatResponseModel>(responseBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Ошибка десериализации");
        }

        if (res.Response is null)
        {
            msg = $"Error `{nameof(GetPropertiesAsync)}`: parseData is null";
            logger.LogError(msg);
            res.AddError(msg);
        }

        return res;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        memoryCache.Remove(_jwt);
        memoryCache.Remove(_requestKeyCache);

        httpClient?.Dispose();
        httpClient = null;
    }
}