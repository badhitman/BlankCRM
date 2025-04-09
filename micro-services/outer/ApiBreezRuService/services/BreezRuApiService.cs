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
/// BreezRuApiService
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
        TResponseModel<List<BreezRuLeftoverModel>> jsonData = await LeftoversGetAsync(token: token);
        if (jsonData.Response is null)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: parseData.Response is null";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }
        logger.LogInformation($"Скачано {jsonData.Response.Count} позиций. Подготовка к записи в БД (удаление старых данных и открытие транзакции)");
        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

        await ctx.Leftovers.ExecuteDeleteAsync(cancellationToken: token);

        BreezRuLeftoverModelDB[] leftovers = [.. jsonData.Response.Select(BreezRuLeftoverModelDB.Build)];

        int _sc = 0;
        foreach (BreezRuLeftoverModelDB[] itemsPart in leftovers.Chunk(10))
        {
            await ctx.AddRangeAsync(itemsPart, token);
            await ctx.SaveChangesAsync(token);
            _sc += itemsPart.Length;
            logger.LogInformation($"Записана очередная порция [{itemsPart.Length}] данных ({_sc}/{leftovers.Length})");
        }

        await transaction.CommitAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}. Записано элементов: {jsonData.Response.Count}");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BrandBreezRuModel>>> GetBrandsAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CategoryBreezRuModel>>> GetCategoriesAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ProductBreezRuModel>>> GetProductsAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechCategoryBreezRuModel>>> GetTechCategoryAsync(TechRequestModel req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<TechProductBreezRuResponseModel>>> GetTechProductAsync(TechRequestModel req, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuLeftoverModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default)
    {
        TResponseModel<List<BreezRuLeftoverModel>> result = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("leftovers/", token);
        logger.LogInformation($"http запрос: {response.RequestMessage}");
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
        result.AddSuccess($"Прочитано товаров: {result.Response.Count}");
        return result;
    }
}