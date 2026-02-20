////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// ProductHaierModelDB
/// </summary>
[Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
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

    /// <summary>
    /// Дата первого появления
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }


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
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

        };
        res.Files = x.Files is null ? null : [.. x.Files.Select(x => FileFeedItemHaierModelDB.Build(x, res))];
        res.SectionsOptions = x.Options is null ? null : [.. x.Options.Select(x => SectionOptionHaierModelDB.Build(x, res))];
        return res;
    }
}