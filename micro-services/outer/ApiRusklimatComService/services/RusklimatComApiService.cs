////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

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

                response = await _httpClient.PostAsync($"api/{_conf.Value.Version}/auth/jwt/", content, token);
                responseBody = await response.Content.ReadAsStringAsync(token);
                await Task.Delay(200, token);
            } while (!response.IsSuccessStatusCode && ++i < 3);

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseBody))
            {
                logger.LogError($"rusklimat http-client auth error: {responseBody}");
                return;
            }

            Dictionary<string, object>? authorizeData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
            if (authorizeData is null || authorizeData.Count == 0)
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
        using HttpResponseMessage _keyRest = await httpClient.GetAsync($"/{_conf.Value.Version}/{_pref}/{_conf.Value.PartnerId}/requestKey/", token);
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
        memoryCache.Set(_requestKeyCache, _rk.RequestKey, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
    }


    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        await GetClient(token: token);
        if (httpClient is null)
            return ResponseBaseModel.CreateError("httpClient is null");

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<UnitsRusklimatResponseModel> GetUnitsAsync(CancellationToken token = default)
    {
        ApiRusklimatComContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<CategoriesRusklimatResponseModel> GetCategoriesAsync(CancellationToken token = default)
    {
        ApiRusklimatComContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<PropertiesRusklimatResponseModel> GetPropertiesAsync(CancellationToken token = default)
    {
        ApiRusklimatComContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ProductsRusklimatResponseModel> GetProductsAsync(PaginationRequestModel req, CancellationToken token = default)
    {
        ApiRusklimatComContext ctx = await dbFactory.CreateDbContextAsync(token);
        throw new NotImplementedException();
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