////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// SectionOptionHaierModelDB
/// </summary>
[Index(nameof(Name))]
public class SectionOptionHaierModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public ProductHaierModelDB? Product { get; set; }
    /// <inheritdoc/>
    public int ProductId { get; set; }

    /// <inheritdoc/>
    public List<OptionHaierModelDB>? Options { get; set; }

    /// <inheritdoc/>
    public static SectionOptionHaierModelDB Build(SectionOptionFeedItemHaierModel x, ProductHaierModelDB p)
    {
        SectionOptionHaierModelDB res = new()
        {
            Name = x.Name,
            Product = p,
            ProductId = p.Id,
        };
        res.Options = [.. x.Options.Select(y => OptionHaierModelDB.Build(y, res))];
        return res;
    }
}