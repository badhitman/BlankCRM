////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using DocumentFormat.OpenXml.InkML;

namespace FeedsHaierProffRuService;

/// <summary>
/// HaierProffRuApiService
/// </summary>
#pragma warning disable CS9113 // Параметр не прочитан.
public class HaierProffRuFeedsService(IHttpClientFactory HttpClientFactory, ILogger<HaierProffRuFeedsService> logger, IDbContextFactory<FeedsHaierProffRuContext> dbFactory)
#pragma warning restore CS9113 // Параметр не прочитан.
    : IFeedsHaierProffRuService
{
    /// <inheritdoc/>
#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
    public async Task<TResponseModel<List<FeedItemHaierModel>>> ProductsFeedGetAsync(CancellationToken token = default)
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
    {
        TResponseModel<List<FeedItemHaierModel>> result = new();
        XDocument xd;
#if !DEBUG
        xd = XDocument.Parse(Encoding.UTF8.GetString(Properties.Resources.Haierproff_feed_demo_rss_xml));
#else
        string msg, responseBody;
        using HttpClient httpClient = HttpClientFactory.CreateClient(HttpClientsNamesOuterEnum.FeedsHaierProffRu.ToString());
        HttpResponseMessage response = await httpClient.GetAsync("cond/?type=partners", token);

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

#endif
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

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DownloadAndSaveAsync(CancellationToken token = default)
    {
        ResponseBaseModel result = new();
        TResponseModel<List<FeedItemHaierModel>> read = await ProductsFeedGetAsync(token);
        result.AddRangeMessages(read.Messages);
        if (read.Response is not null && read.Response.Count != 0)
        {
            FeedsHaierProffRuContext ctx = await dbFactory.CreateDbContextAsync(token);
            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token);

            await ctx.FilesFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            await ctx.OptionsFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            await ctx.SectionsOptionsFeedsRss.ExecuteDeleteAsync(cancellationToken: token);
            await ctx.ProductsFeedsRss.ExecuteDeleteAsync(cancellationToken: token);

            await ctx.AddRangeAsync(read.Response.Select(ProductHaierModelDB.Build), token);
            await ctx.SaveChangesAsync(token);

            await transaction.CommitAsync(token);
        }

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