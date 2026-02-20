////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// OptionHaierModelDB
/// </summary>
public class OptionHaierModelDB : OptionFeedItemHaierModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public SectionOptionHaierModelDB? Section { get; set; }
    /// <inheritdoc/>
    public int SectionId { get; set; }

    /// <inheritdoc/>
    public static OptionHaierModelDB Build(OptionFeedItemHaierModel o, SectionOptionHaierModelDB s)
    {
        return new()
        {
            Name = o.Name,
            Value = o.Value,
            Section = s,
            SectionId = s.Id,
        };
    }
}