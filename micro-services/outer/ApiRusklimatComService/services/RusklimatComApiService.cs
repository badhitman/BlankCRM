////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;
using DbcLib;

namespace ApiRusklimatComService;

/// <summary>
/// b2b.rusklimat - REST API каталога ИП (Интернет Партнер)
/// </summary>
public class RusklimatComApiService(
    IHttpClientFactory HttpClientFactory,
    IRusklimatComApiTransmission russKlimatRemoteRepo,
    IMemoryCache memoryCache,
    IOptions<AuthAlterModel> _conf,
    ILogger<RusklimatComApiService> logger,
#pragma warning disable CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
    IDbContextFactory<ApiRusklimatComContext> dbFactory) : OuterApiBaseServiceImpl(HttpClientFactory), IRusklimatComApiService, IDisposable
#pragma warning restore CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
{
    HttpClient? httpClient;
    string? requestKey;
    const string _jwt = "rusklimat-jwt", _requestKeyCache = "rusklimat-RequestKey", _pref = "InternetPartner";

    /// <inheritdoc/>
    public override string NameTemplateMQ => Path.Combine(GlobalStaticConstants.TransmissionQueueNamePrefix, $"{GlobalStaticConstants.Routes.RUSKLIMAT_CONTROLLER_NAME}-{GlobalStaticConstants.Routes.SYNCHRONIZATION_CONTROLLER_NAME}");

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
    public override async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        TResponseModel<List<RabbitMqManagementResponseModel>> hc = await HealthCheckAsync(token);
        if (hc.Response is null || hc.Response.Sum(x => x.messages) != 0)
            return ResponseBaseModel.CreateWarning($"В очереди есть не выполненные задачи");

        TResponseModel<UnitsRusklimatResponseModel> getUnits = default!;
        TResponseModel<CategoriesRusklimatResponseModel> getCats = default!;
        TResponseModel<PropertiesRusklimatResponseModel> getProps = default!;

        List<ProductRusklimatModel> prodsData = [];

        await Task.WhenAll(
            Task.Run(async () =>
            {
                RusklimatPaginationRequestModel _req = new() { PageNum = 1, PageSize = 1000 };
                TResponseModel<ProductsRusklimatResponseModel> getProds = await GetProductsAsync(_req, token);

                if (getProds.Response?.Data is null)
                    logger.LogError($"Не удалось скачать `{nameof(GetProductsAsync)}`");
                else
                {
                    try
                    {
                        while (getProds.Response?.Data is not null && getProds.Response.Data.Length != 0)
                        {
                            prodsData.AddRange(getProds.Response.Data);
                            logger.LogInformation($"Скачано +{getProds.Response.Data.Length} позиций `{nameof(GetProductsAsync)}` = {prodsData.Count}");
                            _req.PageNum++;
                            getProds = await GetProductsAsync(_req, token);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Ошибка обработки товаров Rusklimat");
                    }
                }
            }, token),
            Task.Run(async () =>
            {
                getUnits = await GetUnitsAsync(token);

                if (getUnits.Response?.Data is null)
                    logger.LogError($"Не удалось скачать `{nameof(GetUnitsAsync)}`");
                else
                    logger.LogInformation($"Скачано {getUnits.Response.Data.Length} позиций `{nameof(GetUnitsAsync)}`");

            }, token),
            Task.Run(async () =>
            {
                getCats = await GetCategoriesAsync(token);

                if (getCats.Response?.Data is null)
                    logger.LogError($"Не удалось скачать `{nameof(GetCategoriesAsync)}`");
                else
                    logger.LogInformation($"Скачано {getCats.Response.Data.Length} позиций `{nameof(GetCategoriesAsync)}`");

            }, token),
            Task.Run(async () =>
            {
                getProps = await GetPropertiesAsync(token);

                if (getProps.Response?.Data is null)
                    logger.LogError($"Не удалось скачать `{nameof(GetPropertiesAsync)}`");
                else
                    logger.LogInformation($"Скачано {getProps.Response.Data.Length} позиций `{nameof(GetPropertiesAsync)}`");

            }, token)
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

        int _sc = 0;
        foreach (PropertyRusklimatModelDB[] propertiesPart in getProps.Response.Data.Chunk(100))
        {
            await ctx.AddRangeAsync(propertiesPart, token);
            await ctx.SaveChangesAsync(token);
            _sc += propertiesPart.Length;
            logger.LogInformation($"Записана очередная порция `{propertiesPart.GetType().Name}` [{propertiesPart.Length}] данных ({_sc}/{getProps.Response.Data.Length})");
        }

        List<ProductRusklimatModelDB> productsDb = [.. prodsData.Select(x => ProductRusklimatModelDB.Build(x, getProps.Response.Data))];

        await transaction.CommitAsync(token);
        await Task.WhenAll(productsDb.Select(p => Task.Run(async () => await russKlimatRemoteRepo.UpdateProductAsync(p, token))));

        return ResponseBaseModel.CreateSuccess($"Обработано");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateProductAsync(ProductRusklimatModelDB req, CancellationToken token = default)
    {
        using ApiRusklimatComContext ctx = await dbFactory.CreateDbContextAsync(token);
        ProductRusklimatModelDB? prodDb = await ctx.Products
            .Include(x => x.Information)
            .Include(x => x.Properties)
            .Include(x => x.Remains)
            .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

        req.UpdatedAt = DateTime.UtcNow;
        string msg;
        if (prodDb is null)
        {
            req.SetLive();
            await ctx.Products.AddAsync(req, token);
            await ctx.SaveChangesAsync(token);
            msg = $"Добавлен новый товар: #{req.Id} '{req.Name}'";
            logger.LogInformation(msg);
            return ResponseBaseModel.CreateSuccess(msg);
        }

        await ctx.Products
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.Brand, req.Brand)
            .SetProperty(p => p.InternetPrice, req.InternetPrice)
            .SetProperty(p => p.ClientPrice, req.ClientPrice)
            .SetProperty(p => p.Description, req.Description)
            .SetProperty(p => p.VendorCode, req.VendorCode)
            .SetProperty(p => p.UpdatedAt, req.UpdatedAt)
            .SetProperty(p => p.Price, req.Price)
            .SetProperty(p => p.NSCode, req.NSCode)
            .SetProperty(p => p.Name, req.Name)
            .SetProperty(p => p.Exclusive, req.Exclusive)
            .SetProperty(p => p.CategoryId, req.CategoryId), cancellationToken: token);

        logger.LogInformation($"Товар #{req.Id} обновлён");

        msg = $"Обновлён товар: #{req.Id} '{req.Name}'";
        logger.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
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
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
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
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
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
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
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
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
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