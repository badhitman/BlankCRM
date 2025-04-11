////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;
using DbcLib;

namespace ApiBreezRuService;

/// <summary>
/// Интеграция API https://api.breez.ru
/// </summary>
public class BreezRuApiService(IHttpClientFactory HttpClientFactory, ILogger<BreezRuApiService> logger, IDbContextFactory<ApiBreezRuContext> dbFactory)
#pragma warning disable CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
    : OuterApiBaseServiceImpl(HttpClientFactory), IBreezRuApiService
#pragma warning restore CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
{
    /// <inheritdoc/>
    public override string NameTemplateMQ => Path.Combine(GlobalStaticConstants.TransmissionQueueNamePrefix, $"{GlobalStaticConstants.Routes.BREEZ_CONTROLLER_NAME}-{GlobalStaticConstants.Routes.SYNCHRONIZATION_CONTROLLER_NAME}");

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        TResponseModel<List<RabbitMqManagementResponseModel>> hc = await HealthCheckAsync(token);
        if (hc.Response is null || hc.Response.Sum(x => x.messages) != 0)
            return ResponseBaseModel.CreateWarning($"В очереди есть не выполненные задачи");

        string msg;

        TResponseModel<List<BreezRuLeftoverModel>>? leftoversJson = null;
        TResponseModel<List<BrandRealBreezRuModel>>? brandsJson = null;
        TResponseModel<List<CategoryRealBreezRuModel>>? categoriesJson = null;
        TResponseModel<List<ProductRealBreezRuModel>>? productsJson = null;

        await Task.WhenAll([
            Task.Run(async () => { leftoversJson = await LeftoversGetAsync(token: token); }, token),
            Task.Run(async () => { brandsJson = await GetBrandsAsync(token); }, token),
            Task.Run(async () => { categoriesJson = await GetCategoriesAsync(token); }, token),
            Task.Run(async () => { productsJson = await GetProductsAsync(token); }, token),
            ]);

        if (leftoversJson?.Response is null || !leftoversJson.Success() || leftoversJson.Response.Count == 0)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: {nameof(leftoversJson)}.Response is null || !{nameof(leftoversJson)}.Success() || {nameof(leftoversJson)}.Response.Count == 0";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        if (brandsJson?.Response is null || !brandsJson.Success() || brandsJson.Response.Count == 0)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: {nameof(brandsJson)}.Response is null || !{nameof(brandsJson)}.Success() || {nameof(brandsJson)}.Response.Count == 0";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        if (categoriesJson?.Response is null || !categoriesJson.Success() || categoriesJson.Response.Count == 0)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: {nameof(categoriesJson)}.Response is null || !{nameof(categoriesJson)}.Success() || {nameof(categoriesJson)}.Response.Count == 0";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        if (productsJson?.Response is null || !productsJson.Success() || productsJson.Response.Count == 0)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: {nameof(productsJson)}.Response is null || !{nameof(productsJson)}.Success() || {nameof(productsJson)}.Response.Count == 0";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        List<TechCategoryRealBreezRuModel> techForCatDump = [];
        List<TechProductRealBreezRuModel> techForProdDump = [];

        List<string> _errors = [];

        await Task.WhenAll([
            Task.Run(async () => {

        foreach (CategoryRealBreezRuModel _cat in categoriesJson.Response)
        {
            TResponseModel<List<TechCategoryRealBreezRuModel>> techForCat = await GetTechCategoryAsync(new() { Id = _cat.Key }, token);
            if (techForCat?.Response is null || !techForCat.Success() || techForCat.Response.Count == 0)
            {
                msg = $"Error `{nameof(DownloadAndSaveAsync)}`(id = {_cat.Key}): {nameof(techForCat)}.Response is null || !{nameof(techForCat)}.Success() || {nameof(techForCat)}.Response.Count == 0";
                logger.LogWarning(msg);
                        lock(_errors)
                        {
                            _errors.Add(msg);
                        }
            }
            else
                techForCatDump.AddRange(techForCat.Response);
        }

            }, token),
            Task.Run(async () => {

        foreach (ProductRealBreezRuModel _pr in productsJson.Response)
        {
            TResponseModel<List<TechProductRealBreezRuModel>> techForProd = await GetTechProductAsync(new TechRequestModel() { Id = _pr.Key }, token);
            if (techForProd?.Response is null || !techForProd.Success() || techForProd.Response.Count == 0)
            {
                msg = $"Error `{nameof(DownloadAndSaveAsync)}`(id = {_pr.Key}): {nameof(techForProd)}.Response is null || !{nameof(techForProd)}.Success() || {nameof(techForProd)}.Response.Count == 0";
                logger.LogWarning(msg);
                lock(_errors)
                        {
                            _errors.Add(msg);
                        }
            }
            else
                techForProdDump.AddRange(techForProd.Response);
        }

            }, token),
            ]);

        if (_errors.Count != 0)
            logger.LogWarning($"Во время загрузки были зарегистрированы ошибки: {_errors.Count}");

        if (techForCatDump.Count == 0)
        {
            msg = $"Error `{nameof(GetTechCategoryAsync)}`: {nameof(techForCatDump)}.Count == 0";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }
        if (techForProdDump.Count == 0)
        {
            msg = $"Error `{nameof(GetTechProductAsync)}`: {nameof(techForProdDump)}.Count == 0";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }

        logger.LogInformation($"Скачано. Подготовка к записи в БД (удаление старых данных и открытие транзакции)");
        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

        await ctx.Leftovers.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Brands.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Categories.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.ImagesProducts.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.Products.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.TechsProducts.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.TechsCategories.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.PropsTechsCategories.ExecuteDeleteAsync(cancellationToken: token);
        await ctx.PropsTechsProducts.ExecuteDeleteAsync(cancellationToken: token);

        BreezRuLeftoverModelDB[] leftovers = [.. leftoversJson.Response.Select(BreezRuLeftoverModelDB.Build)];

        int _sc = 0;
        foreach (BreezRuLeftoverModelDB[] leftoversPart in leftovers.Chunk(100))
        {
            await ctx.AddRangeAsync(leftoversPart, token);
            await ctx.SaveChangesAsync(token);
            _sc += leftoversPart.Length;
            logger.LogInformation($"Записана очередная порция `{nameof(leftoversPart)}` данных {leftoversPart.Length} ({_sc}/{leftovers.Length})");
        }
        _sc = 0;
        foreach (BrandRealBreezRuModel[] brandsPart in brandsJson.Response.Chunk(100))
        {
            await ctx.AddRangeAsync(brandsPart.Select(BrandBreezRuModelDB.Build), token);
            await ctx.SaveChangesAsync(token);
            _sc += brandsPart.Length;
            logger.LogInformation($"Записана очередная порция `{nameof(brandsPart)}` данных {brandsPart.Length} ({_sc}/{brandsJson.Response.Count})");
        }
        _sc = 0;
        foreach (CategoryRealBreezRuModel[] categoriesPart in categoriesJson.Response.Chunk(100))
        {
            await ctx.AddRangeAsync(categoriesPart.Select(CategoryBreezRuModelDB.Build), token);
            await ctx.SaveChangesAsync(token);
            _sc += categoriesPart.Length;
            logger.LogInformation($"Записана очередная порция `{nameof(categoriesPart)}` данных {categoriesPart.Length} ({_sc}/{categoriesJson.Response.Count})");
        }
        _sc = 0;
        foreach (ProductRealBreezRuModel[] productsPart in productsJson.Response.Chunk(100))
        {
            await ctx.AddRangeAsync(productsPart.Select(ProductBreezRuModelDB.Build), token);
            await ctx.SaveChangesAsync(token);
            _sc += productsPart.Length;
            logger.LogInformation($"Записана очередная порция `{nameof(productsPart)}` данных {productsPart.Length} ({_sc}/{productsJson.Response.Count})");
        }
        _sc = 0;
        foreach (TechCategoryRealBreezRuModel[] _txCatsPart in techForCatDump.Chunk(100))
        {
            try
            {
                List<TechCategoryBreezRuModelDB> tcList = [.. _txCatsPart.Select(TechCategoryBreezRuModelDB.Build)];
                await ctx.AddRangeAsync(tcList, token);
                await ctx.SaveChangesAsync(token);
                _sc += _txCatsPart.Length;
                logger.LogInformation($"Записана очередная порция `{nameof(_txCatsPart)}` данных {_txCatsPart.Length} ({_sc}/{techForCatDump.Count})");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        _sc = 0;
        foreach (TechProductRealBreezRuModel[] _txProdsPart in techForProdDump.Chunk(100))
        {
            try
            {
                List<TechProductBreezRuModelDB> tpList = [.. _txProdsPart.Select(TechProductBreezRuModelDB.Build)];
                await ctx.AddRangeAsync(tpList, token);
                await ctx.SaveChangesAsync(token);
                _sc += _txProdsPart.Length;
                logger.LogInformation($"Записана очередная порция `{nameof(_txProdsPart)}` данных {_txProdsPart.Length} ({_sc}/{techForProdDump.Count})");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}. Записано элементов: {leftoversJson.Response.Count}");
    }


    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryRealBreezRuModel>>> GetTechCategoryAsync(TechRequestModel req, CancellationToken token = default)
    {
        TResponseModel<List<TechCategoryRealBreezRuModel>> result = new();

        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"tech/?category={req.Id}", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        string msg;
        string responseBody = await response.Content.ReadAsStringAsync(token);

        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetTechCategoryAsync)}` (http code: {response.StatusCode}): {responseBody}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetTechCategoryAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }

        TechCategoryResponseBreezRuModel? _techs = JsonConvert.DeserializeObject<TechCategoryResponseBreezRuModel>(responseBody);
        if (_techs?.Techs is null)
        {
            msg = $"Error `{nameof(GetTechCategoryAsync)}`({nameof(req.Id)}:{req.Id}): Dictionary<string, TechCategoryBreezRuModel> techs is null\n{responseBody}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        result.Response = [.. _techs.Techs.Select(x => TechCategoryRealBreezRuModel.Build(x, _techs.Category))];
        if (result.Response is null)
        {
            msg = $"Error `{nameof(GetTechCategoryAsync)}`: parseData is null";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        msg = $"Прочитано [Технические характеристики Категории #{req.Id}]: {result.Response.Sum(x => x.Techs is null ? 0 : x.Techs.Count)}";
        result.AddSuccess(msg);
        logger.LogInformation(msg);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechProductRealBreezRuModel>>> GetTechProductAsync(TechRequestModel req, CancellationToken token = default)
    {
        TResponseModel<List<TechProductRealBreezRuModel>> result = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync($"tech/?id={req.Id}", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        string msg;
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetTechProductAsync)}` (http code: {response.StatusCode}): {responseBody}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetTechProductAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }

        Dictionary<int, TechProductBreezRuResponseModel>? _techs = JsonConvert.DeserializeObject<Dictionary<int, TechProductBreezRuResponseModel>>(responseBody);
        if (_techs is null)
        {
            msg = $"Error `{nameof(GetTechProductAsync)}`({nameof(req.Id)}:{req.Id}): techs is null\n{responseBody}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        result.Response = [.. _techs.Select(TechProductRealBreezRuModel.Build)];
        msg = $"Прочитано [Технические характеристики Продукта #{req.Id}]: {result.Response.Sum(x => (x.Techs is null ? 0 : x.Techs.Count))}";
        result.AddSuccess(msg);
        logger.LogInformation(msg);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BrandRealBreezRuModel>>> GetBrandsAsync(CancellationToken token = default)
    {
        TResponseModel<List<BrandRealBreezRuModel>> result = new();

        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("brands/", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetBrandsAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetBrandsAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }

        result.Response = [.. JsonConvert.DeserializeObject<Dictionary<string, BrandBreezRuModel>>(responseBody)!.Select(BrandRealBreezRuModel.Build)];

        if (result.Response is null)
        {
            msg = $"Error `{nameof(GetBrandsAsync)}`: parseData is null";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        msg = $"Прочитано брэндов: {result.Response.Count}";
        logger.LogInformation(msg);
        result.AddSuccess(msg);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CategoryRealBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default)
    {
        TResponseModel<List<CategoryRealBreezRuModel>> result = new();

        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("categories/", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetCategoriesAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetCategoriesAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        Dictionary<int, CategoryBreezRuModel>? parseData = JsonConvert.DeserializeObject<Dictionary<int, CategoryBreezRuModel>>(responseBody);
        if (parseData is null)
        {
            msg = $"Error `{nameof(GetCategoriesAsync)}`: parseData is null";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        result.Response = [.. parseData.Select(x => CategoryRealBreezRuModel.Build(x))];

        msg = $"Прочитано категорий: {result.Response.Count}";
        result.AddSuccess(msg);
        logger.LogInformation(msg);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductRealBreezRuModel>>> GetProductsAsync(CancellationToken token = default)
    {
        TResponseModel<List<ProductRealBreezRuModel>> result = new();

        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("products/", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(GetProductsAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(GetProductsAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        Dictionary<int, ProductBreezRuModel>? parseData = JsonConvert.DeserializeObject<Dictionary<int, ProductBreezRuModel>>(responseBody);
        if (parseData is null)
        {
            msg = $"Error `{nameof(GetProductsAsync)}`: parseData is null";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        result.Response = [.. parseData.Select(ProductRealBreezRuModel.Build)];

        msg = $"Прочитано товаров: {result.Response.Count}";
        result.AddSuccess(msg);
        logger.LogInformation(msg);
        return result;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuLeftoverModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default)
    {
        TResponseModel<List<BreezRuLeftoverModel>> result = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("leftovers/", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        string msg;
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        string responseBody = await response.Content.ReadAsStringAsync(token);
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }

        result.Response = JsonConvert.DeserializeObject<List<BreezRuLeftoverModel>>(responseBody);
        if (result.Response is null)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: parseData is null";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        msg = $"Прочитано остатков: {result.Response.Count}";
        result.AddSuccess(msg);
        logger.LogInformation(msg);
        return result;
    }
}