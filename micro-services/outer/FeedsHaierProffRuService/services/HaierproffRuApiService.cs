////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;
using System.Xml.Linq;
using RemoteCallLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace FeedsHaierProffRuService;

/// <summary>
/// RSS feed HaierProff
/// </summary>
public class HaierProffRuFeedsService(IHttpClientFactory HttpClientFactory, ILogger<HaierProffRuFeedsService> logger, IDbContextFactory<FeedsHaierProffRuContext> dbFactory)
#pragma warning disable CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
    : OuterApiBaseServiceImpl(HttpClientFactory), IFeedsHaierProffRuService
#pragma warning restore CS9107 // Параметр записан в состоянии включающего типа, а его значение также передается базовому конструктору. Значение также может быть записано базовым классом.
{
    /// <inheritdoc/>
    public override string NameTemplateMQ => Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueueNamePrefix, $"{Routes.HAIERPROFF_CONTROLLER_NAME}-{Routes.SYNCHRONIZATION_CONTROLLER_NAME}");

    /// <inheritdoc/>
    public override async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        TResponseModel<List<RabbitMqManagementResponseModel>> hc = await HealthCheckAsync(token);
        if (hc.Response is null || hc.Response.Sum(x => x.messages) != 0)
            return ResponseBaseModel.CreateWarning($"В очереди есть не выполненные задачи");

        ResponseBaseModel result = new();

        TResponseModel<List<FeedItemHaierModel>> read = await ProductsFeedGetAsync(token);
        result.AddRangeMessages(read.Messages);
        if (read.Response is not null && read.Response.Count != 0)
        {
            using FeedsHaierProffRuContext ctx = await dbFactory.CreateDbContextAsync(token);
            logger.LogInformation($"Скачано {read.Response.Count} позиций. Подготовка к записи в БД (удаление старых данных и открытие транзакции)");
            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

            await ctx.FilesFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            await ctx.OptionsFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            await ctx.SectionsOptionsFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            await ctx.ProductsFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            int _sc = 0;
            foreach (FeedItemHaierModel[] itemsPart in read.Response.Chunk(100))
            {
                await ctx.AddRangeAsync(itemsPart.Select(ProductHaierModelDB.Build), token);
                await ctx.SaveChangesAsync(token);
                _sc += itemsPart.Length;
                logger.LogInformation($"Записана очередная порция `{itemsPart.GetType().Name}` [{itemsPart.Length}] данных ({_sc}/{read.Response.Count})");
            }

            logger.LogInformation($"Данные записаны в БД. Закрытие транзакции.");
            await transaction.CommitAsync(token);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ProductHaierModelDB>> ProductsSelectAsync(HaierRequestModel req, CancellationToken token = default)
    {
        using FeedsHaierProffRuContext ctx = await dbFactory.CreateDbContextAsync(token);
        return await ctx.ProductsSelectAsync(req, token);
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<List<FeedItemHaierModel>>> ProductsFeedGetAsync(CancellationToken token = default)
    {
        TResponseModel<List<FeedItemHaierModel>> result = new();
        XDocument xd;

        string msg, responseBody;
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.FeedsHaierProffRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("cond/?type=partners", token);
        logger.LogInformation($"http запрос: {response.RequestMessage?.RequestUri}");
        if (!response.IsSuccessStatusCode)
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}` (http code: {response.StatusCode}): {await response.Content.ReadAsStringAsync(token)}";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        responseBody = await response.Content.ReadAsStringAsync(token);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            msg = $"Error `{nameof(DownloadAndSaveAsync)}`: string.IsNullOrWhiteSpace(responseBody)";
            logger.LogError(msg);
            result.AddError(msg);
            return result;
        }
        responseBody = responseBody.Replace("&ndash;", "-").Replace("&shy;", "");
        xd = XDocument.Parse(responseBody);

        if (xd.Root is null)
        {
            result.AddError("XDocument.Root is null");
            return result;
        }

        result.Response = [.. xd.Root.Descendants()
                      .First(i => i.Name.LocalName == "channel")
                      .Elements()
                      .Where(i => i.Name.LocalName == "item").Select(item =>
                      {
                          XElement[] els = [.. item.Elements()];

                          FeedItemHaierModel _el = new()
                          {
                              Name = els.First(i => i.Name.LocalName == "name").Value,
                              Url = els.First(i => i.Name.LocalName == "url").Value,
                              Price = els.First(i => i.Name.LocalName == "price").Value,
                              Category = els.First(i => i.Name.LocalName == "category").Value,
                              ParentCategory = els.First(i => i.Name.LocalName == "parentcategory").Value,
                              AllArticles = els.First(i => i.Name.LocalName == "all_articles").Value,
                              ImageLink = els.First(i => i.Name.LocalName == "imagelink").Value,
                              Description = els.First(i => i.Name.LocalName == "description").Value,
                          Options = ReadOptions(els.FirstOrDefault(i => i.Name.LocalName == "options")?.Elements()),
                          Files = GetFiles(els.FirstOrDefault(i => i.Name.LocalName == "files")?.Elements()) };

                          return _el;
                      })];

        result.AddSuccess($"Из RSS прочитано: {result.Response.Count}");
        return result;
    }


    static List<SectionOptionFeedItemHaierModel>? ReadOptions(IEnumerable<XElement>? enumerable)
    {
        if (enumerable is null)
            return null;

        return [.. enumerable.Where(i => i.Name.LocalName == "section")
            .Select(item =>
            {
                XElement[] els = [.. item.Elements()];
                return new SectionOptionFeedItemHaierModel()
                {
                    Name = els.First(i => i.Name.LocalName == "name").Value,
                    Options = GetOptions(els.FirstOrDefault(i => i.Name.LocalName == "options")?.Elements())
                };
            })];
    }

    static List<OptionFeedItemHaierModel> GetOptions(IEnumerable<XElement>? enumerable)
    {
        if (enumerable is null)
            return [];

        return [.. enumerable.Where(i => i.Name.LocalName == "option")
            .Select(item =>
            {
                XElement[] els = [.. item.Elements()];
                return new OptionFeedItemHaierModel()
                {
                    Name = els.First(i => i.Name.LocalName == "name").Value,
                     Value = els.First(i => i.Name.LocalName == "value").Value
                };
            })];
    }

    static List<FileFeedItemHaierModel>? GetFiles(IEnumerable<XElement>? enumerable)
    {
        if (enumerable is null)
            return null;

        return [.. enumerable.Where(i => i.Name.LocalName == "file")
            .Select(item =>
            {
                XElement[] els = [.. item.Elements()];
                return new FileFeedItemHaierModel()
                {
                    Name = els.First(i => i.Name.LocalName == "name").Value,
                    Url = els.First(i => i.Name.LocalName == "url").Value
                };
            })];
    }
}