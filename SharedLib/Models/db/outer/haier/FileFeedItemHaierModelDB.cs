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
    public FeedItemHaierModelDB? Product { get; set; }
    /// <inheritdoc/>
    public int ProductId { get; set; }
}