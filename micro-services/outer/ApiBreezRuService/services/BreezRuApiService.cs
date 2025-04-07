////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace ApiBreezRuService;

/// <summary>
/// BreezRuApiService
/// </summary>
public class BreezRuApiService(IHttpClientFactory HttpClientFactory, ILogger<BreezRuApiService> logger, IDbContextFactory<ApiBreezRuContext> dbFactory) : IBreezRuApiService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        string msg;
        TResponseModel<List<BreezRuGoodsModel>> jsonData = await LeftoversGetAsync(token: token);
        if (jsonData.Response is null)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: parseData.Response is null";
            logger.LogError(msg);
            return ResponseBaseModel.CreateError(msg);
        }
        using ApiBreezRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

        await ctx.Goods.ExecuteDeleteAsync(cancellationToken: token);

        await ctx.AddRangeAsync(jsonData.Response.Select(BreezRuElementModelDB.Build), token);
        await ctx.SaveChangesAsync(token);

        await transaction.CommitAsync(token);

        return ResponseBaseModel.CreateSuccess($"Задание выполнено: {nameof(DownloadAndSaveAsync)}. Записано элементов: {jsonData.Response.Count}");
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<BreezRuGoodsModel>>> LeftoversGetAsync(string? nc = null, CancellationToken token = default)
    {
        TResponseModel<List<BreezRuGoodsModel>> result = new();
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.ApiBreezRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("leftovers/", token);
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

        result.Response = JsonConvert.DeserializeObject<List<BreezRuGoodsModel>>(responseBody);

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