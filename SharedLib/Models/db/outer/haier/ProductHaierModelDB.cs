////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ProductHaierModelDB
/// </summary>
public class ProductHaierModelDB : FeedItemHaierBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public List<SectionOptionHaierModelDB>? SectionsOptions { get; set; }

    /// <inheritdoc/>
    public List<FileFeedItemHaierModelDB>? Files { get; set; }

    /// <inheritdoc/>
    public static ProductHaierModelDB Build(FeedItemHaierModel x)
    {
        ProductHaierModelDB res = new()
        {
            Category = x.Category,
            Description = x.Description,
            Name = x.Name,
            Price = x.Price,
            Url = x.Url,
            AllArticles = x.AllArticles,
            ImageLink = x.ImageLink,
            ParentCategory = x.ParentCategory,
        };
        res.Files = x.Files is null ? null :[.. x.Files.Select(x => FileFeedItemHaierModelDB.Build(x, res))];
        res.SectionsOptions = x.Options is null ? null : [.. x.Options.Select(x => SectionOptionHaierModelDB.Build(x, res))];
        return res;
    }
}