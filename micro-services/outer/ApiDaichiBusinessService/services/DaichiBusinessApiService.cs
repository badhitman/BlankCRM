////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;
using RemoteCallLib;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ApiDaichiBusinessService;

/// <summary>
/// API Daichi Бизнес (daichi.business)
/// </summary>
public class DaichiBusinessApiService(IHttpClientFactory HttpClientFactory,
    IOptions<TokenVersionModel> _conf,
    ILogger<DaichiBusinessApiService> logger,
    IDaichiBusinessApiTransmission daichiTransmission,
#pragma warning disable CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
    IDbContextFactory<ApiDaichiBusinessContext> dbFactory) : OuterApiBaseServiceImpl(HttpClientFactory), IDaichiBusinessApiService
#pragma warning restore CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
{
    /// <inheritdoc/>
    public override string NameTemplateMQ => Path.Combine(GlobalStaticConstants.TransmissionQueueNamePrefix, $"{GlobalStaticConstants.Routes.DAICHI_CONTROLLER_NAME}-{GlobalStaticConstants.Routes.SYNCHRONIZATION_CONTROLLER_NAME}");

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        TResponseModel<List<RabbitMqManagementResponseModel>> hc = await HealthCheckAsync(token);
        if (hc.Response is null || hc.Response.Sum(x => x.messages) != 0)
            return ResponseBaseModel.CreateWarning($"В очереди есть не выполненные задачи");

        TResponseModel<ProductsDaichiBusinessResultModel> products = default!;
        TResponseModel<StoresDaichiBusinessResponseModel> stores = default!;
        List<ParameterElementDaichiJsonModel> parametersAll = [];
        await Task.WhenAll([
            Task.Run(async () =>
            {
                ProductParamsRequestDaichiModel _rq = new (){ PageSize = 100, Page = 1 };
                TResponseModel<ProductsParamsDaichiBusinessResponseModel> productsParameters = await ProductsParamsGetAsync(_rq, token);

                if(productsParameters.Response?.Result?.Data is null)
                    logger.LogError($"Не удалось скачать `{nameof(ProductsParamsGetAsync)}`");
                else
                {
                    try
                    {
                        while(productsParameters.Response?.Result?.Data is not null && productsParameters.Response.Result.Data.Count != 0)
                        {
                            parametersAll.AddRange( productsParameters.Response.Result.Data.Values.Select(x =>
                            {
                                ParameterElementDaichiJsonModel res = x.ToObject<ParameterElementDaichiJsonModel>()!;
                                JProperty[] _jpts = [.. x.Properties().Where(x => x.Name.StartsWith("attr_", StringComparison.OrdinalIgnoreCase))];
                                res.Attributes = [.. _jpts.Select(x => x.First().ToObject<AttributeParameterDaichiModel>())];
                                return res;
                            }));
                            _rq.Page++;
                            logger.LogInformation($"Скачано +{productsParameters.Response.Result.Data.Count} позиций `{nameof(ProductsParamsGetAsync)}` = {parametersAll.Count}");
                            productsParameters = await ProductsParamsGetAsync(_rq, token);
                        }
                    }
                    catch(Exception ex){
                        logger.LogError(ex, "Ошибка обработки товаров Daichi");
                    }
                }
            }, token),
            Task.Run(async () =>
            {
                products = await ProductsGetAsync(new ProductsRequestDaichiModel(), token);
                if(products.Response?.Result is null)
                    logger.LogError($"Не удалось скачать `{nameof(ProductsGetAsync)}`");
                else
                    logger.LogInformation($"Скачано {products.Response.Result.Count} позиций `{nameof(ProductsGetAsync)}`");
            }, token),
            Task.Run(async () =>
            {
                stores = await StoresGetAsync(token);
                if(stores.Response?.Result is null)
                    logger.LogError($"Не удалось скачать `{nameof(StoresGetAsync)}`");
                else
                    logger.LogInformation($"Скачано {stores.Response.Result.Count} позиций `{nameof(StoresGetAsync)}`");
            }, token)
            ]);

        if (stores.Response?.Result is null)
            return ResponseBaseModel.CreateError("stores.Response?.Result is null");

        if (products.Response?.GetProducts is null)
            return ResponseBaseModel.CreateError("products.Response?.GetProducts");

        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);

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
        List<ParameterEntryDaichiModelDB> productsParametersDb = [.. parametersAll.Select(x => ParameterEntryDaichiModelDB.Build(x, productsDb))];

        logger.LogInformation($"Данные записаны в БД. Закрытие транзакции.");
        await transaction.CommitAsync(token);

        await Task.WhenAll
            (
            productsParametersDb.Select(p => Task.Run(async () => await daichiTransmission.ParameterUpdateAsync(p, token), token))
            .Union(productsDb.Select(g => Task.Run(async () => await daichiTransmission.ProductUpdateAsync(g, token), token)))
            );

        if (products.Response.Exceptions != null && !products.Response.Exceptions.IsEmpty)
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
    public async Task<ResponseBaseModel> ParameterUpdateAsync(ParameterEntryDaichiModelDB req, CancellationToken token = default)
    {
        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);
        ParameterEntryDaichiModelDB? prodDb = await ctx.ParametersCatalog
          .Include(x => x.Attributes)
          .Include(x => x.Photos)
          .Include(x => x.Sections)
          .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

        req.UpdatedAt = DateTime.UtcNow;
        string msg;
        if (prodDb is null)
        {
            req.SetLive();
            await ctx.ParametersCatalog.AddAsync(req, token);
            await ctx.SaveChangesAsync(token);
            msg = $"Добавлен новый параметр (каталог): #{req.XML_ID} '{req.NAME}'";
            logger.LogInformation(msg);
            return ResponseBaseModel.CreateSuccess(msg);
        }

        await ctx.ParametersCatalog
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.NAME, req.NAME)
            .SetProperty(p => p.BRAND, req.BRAND)
            .SetProperty(p => p.ID, req.ID)
            .SetProperty(p => p.XML_ID, req.XML_ID)
            .SetProperty(p => p.MAIN_SECTION, req.MAIN_SECTION)
            .SetProperty(p => p.UpdatedAt, req.UpdatedAt), cancellationToken: token);

        logger.LogInformation($"Параметр (каталог) #{req.Id} обновлён");

        msg = $"Обновлён параметр (каталог): #{req.XML_ID} '{req.NAME}'";
        logger.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ProductUpdateAsync(ProductDaichiModelDB req, CancellationToken token = default)
    {
        using ApiDaichiBusinessContext ctx = await dbFactory.CreateDbContextAsync(token);
        ProductDaichiModelDB? prodDb = await ctx.Products
            .Include(x => x.Params)
            .Include(x => x.Prices)
            .Include(x => x.StoreAvailability)
            .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

        req.UpdatedAt = DateTime.UtcNow;
        string msg;
        if (prodDb is null)
        {
            req.SetLive();
            await ctx.Products.AddAsync(req, token);
            await ctx.SaveChangesAsync(token);
            msg = $"Добавлен новый товар: #{req.XML_ID} '{req.NAME}'";
            logger.LogInformation(msg);
            return ResponseBaseModel.CreateSuccess(msg);
        }

        await ctx.Products
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.NAME, req.NAME)
            .SetProperty(p => p.KeyIndex, req.KeyIndex)
            .SetProperty(p => p.XML_ID, req.XML_ID)
            .SetProperty(p => p.UpdatedAt, req.UpdatedAt), cancellationToken: token);

        logger.LogInformation($"Товар #{req.Id} обновлён");

        msg = $"Обновлён товар: #{req.XML_ID} '{req.NAME}'";
        logger.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<ProductsDaichiBusinessResultModel>> ProductsGetAsync(ProductsRequestDaichiModel req, CancellationToken token = default)
    {
        TResponseModel<ProductsDaichiBusinessResultModel> res = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiDaichiBusiness.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"/products/get/?access-token={_conf.Value.Token}&store-id={req.StoreId}", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
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
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
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
            msg = $"Ошибка десериализации:\n{responseBody}";
            logger.LogError(ex, msg);
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