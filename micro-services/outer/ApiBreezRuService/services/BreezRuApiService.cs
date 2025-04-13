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
public class BreezRuApiService(IHttpClientFactory HttpClientFactory,
    ILogger<BreezRuApiService> logger,
    IBreezRuApiTransmission breexTransmission,
    IDbContextFactory<ApiBreezRuContext> dbFactory)
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
            TResponseModel<List<TechProductRealBreezRuModel>> techForProd = await GetTechProductAsync(new TechRequestBreezModel() { Id = _pr.Key }, token);
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
        foreach (TechCategoryRealBreezRuModel[] _txCatsPart in techForCatDump.Chunk(100))
        {
            List<TechCategoryBreezRuModelDB> tcList = [.. _txCatsPart.Select(TechCategoryBreezRuModelDB.Build)];
            await ctx.AddRangeAsync(tcList, token);
            await ctx.SaveChangesAsync(token);
            _sc += _txCatsPart.Length;
            logger.LogInformation($"Записана очередная порция `{nameof(_txCatsPart)}` данных {_txCatsPart.Length} ({_sc}/{techForCatDump.Count})");
        }

        await transaction.CommitAsync(token);

        foreach (ProductBreezRuModelDB p in productsJson.Response.Select(ProductBreezRuModelDB.Build))
            await breexTransmission.ProductUpdateAsync(p, token);

        foreach (TechProductBreezRuModelDB tp in techForProdDump.Select(TechProductBreezRuModelDB.Build))
            await breexTransmission.TechProductUpdateAsync(tp, token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}. Записано элементов: {leftoversJson.Response.Count}");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> TechProductUpdateAsync(TechProductBreezRuModelDB req, CancellationToken token = default)
    {
        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        TechProductBreezRuModelDB? prodDb = await ctx.TechsProducts
          .Include(x => x.Properties)
          .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

        req.UpdatedAt = DateTime.UtcNow;
        req.CreatedAt = DateTime.UtcNow;
        string msg;
        if (prodDb is null)
        {
            req.SetLive();
            await ctx.TechsProducts.AddAsync(req, token);
            await ctx.SaveChangesAsync(token);
            msg = $"Добавлен новый tech-атрибут: #{req.NC}";
            logger.LogInformation(msg);
            return ResponseBaseModel.CreateSuccess(msg);
        }

        await ctx.TechsProducts
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.NarujNC, req.NarujNC)
            .SetProperty(p => p.VnutrNC, req.VnutrNC)
           .SetProperty(p => p.AccessoryNC, req.AccessoryNC)
            .SetProperty(p => p.NC, req.NC)
            .SetProperty(p => p.UpdatedAt, req.UpdatedAt), cancellationToken: token);

        logger.LogInformation($"tech-атрибут обновлён");
        msg = $"Обновлён tech-атрибут: #{req.Id}";
        logger.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ProductUpdateAsync(ProductBreezRuModelDB req, CancellationToken token = default)
    {
        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        ProductBreezRuModelDB? prodDb = await ctx.Products
          .Include(x => x.Images)
          .FirstOrDefaultAsync(x => x.Id == req.Id, cancellationToken: token);

        req.UpdatedAt = DateTime.UtcNow;
        req.CreatedAt = DateTime.UtcNow;
        string msg;
        if (prodDb is null)
        {
            req.SetLive();
            await ctx.Products.AddAsync(req, token);
            await ctx.SaveChangesAsync(token);
            msg = $"Добавлен новый товар: #{req.NC} '{req.Title}'";
            logger.LogInformation(msg);
            return ResponseBaseModel.CreateSuccess(msg);
        }

        await ctx.Products
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
            .SetProperty(p => p.NC, req.NC)
            .SetProperty(p => p.Manual, req.Manual)
            .SetProperty(p => p.NarujNC, req.NarujNC)
            .SetProperty(p => p.VnutrNC, req.VnutrNC)
            .SetProperty(p => p.AccessoryNC, req.AccessoryNC)
            .SetProperty(p => p.Article, req.Article)
            .SetProperty(p => p.BimModel, req.BimModel)
            .SetProperty(p => p.VideoYoutube, req.VideoYoutube)
            .SetProperty(p => p.UTP, req.UTP)
            .SetProperty(p => p.Title, req.Title)
            .SetProperty(p => p.Series, req.Series)
            .SetProperty(p => p.PriceRIC, req.PriceRIC)
            .SetProperty(p => p.PriceCurrencyRIC, req.PriceCurrencyRIC)
            .SetProperty(p => p.Description, req.Description)
            .SetProperty(p => p.CategoryId, req.CategoryId)
            .SetProperty(p => p.Brand, req.Brand)
            .SetProperty(p => p.Booklet, req.Booklet)
            .SetProperty(p => p.UpdatedAt, req.UpdatedAt), cancellationToken: token);

        msg = $"Обновлён товар: #{req.NC} [{req.Id}] '{req.Title}'";
        logger.LogInformation(msg);
        return ResponseBaseModel.CreateSuccess(msg);
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductViewBreezRuModeld>> ProductsSelectAsync(BreezRequestModel req, CancellationToken token = default)
    {
        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        return await ctx.ProductsSelect(req, token);
    }


    #region rest/api
    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryRealBreezRuModel>>> GetTechCategoryAsync(TechRequestBreezModel req, CancellationToken token = default)
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
    public async Task<TResponseModel<List<TechProductRealBreezRuModel>>> GetTechProductAsync(TechRequestBreezModel req, CancellationToken token = default)
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
    #endregion
}