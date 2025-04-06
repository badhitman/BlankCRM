////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OptionFeedItemHaierModelDB
/// </summary>
public class OptionFeedItemHaierModelDB : OptionFeedItemHaierModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public SectionOptionFeedItemHaierModelDB? Section { get; set; }
    /// <inheritdoc/>
    public int SectionId { get; set; }
}