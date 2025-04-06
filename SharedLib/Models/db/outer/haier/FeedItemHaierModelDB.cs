////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// FeedItemHaierModelDB
/// </summary>
public class FeedItemHaierModelDB : FeedItemHaierBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public List<SectionOptionFeedItemHaierModelDB>? SectionsOptions { get; set; }

    /// <inheritdoc/>
    public List<FileFeedItemHaierModelDB>? Files {  get; set; }
}