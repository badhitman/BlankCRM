////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechCategoryBreezRuModelDB
/// </summary>
public class TechCategoryBreezRuModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// CategoryId
    /// </summary>
    public required int CategoryId { get; set; }

    /// <summary>
    /// Properties
    /// </summary>
    public List<TechPropertyCategoryBreezRuModelDB>? Properties { get; set; }
}