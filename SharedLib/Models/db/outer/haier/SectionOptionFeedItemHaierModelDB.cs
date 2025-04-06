////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// SectionOptionFeedItemHaierModelDB
/// </summary>
public class SectionOptionFeedItemHaierModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public FeedItemHaierModelDB? Product { get; set; }
    /// <inheritdoc/>
    public int ProductId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<OptionFeedItemHaierModelDB>? Options { get; set; }
}