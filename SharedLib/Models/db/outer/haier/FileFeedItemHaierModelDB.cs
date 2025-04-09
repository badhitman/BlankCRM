////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// FileFeedItemHaierModelDB
/// </summary>
public class FileFeedItemHaierModelDB : FileFeedItemHaierModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }


    /// <inheritdoc/>
    public ProductHaierModelDB? Product { get; set; }
    /// <inheritdoc/>
    public int ProductId { get; set; }

    /// <inheritdoc/>
    public static FileFeedItemHaierModelDB Build(FileFeedItemHaierModel x, ProductHaierModelDB p)
    {
        return new FileFeedItemHaierModelDB()
        {
            Name = x.Name,
            Url = x.Url,
            Product = p,
            ProductId = p.Id,
        };
    }
}