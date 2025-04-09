////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechPropertyCategoryBreezRuModelDB
/// </summary>
public class TechPropertyCategoryBreezRuModelDB : TechCategoryBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Parent
    /// </summary>
    public TechCategoryBreezRuModelDB? Parent { get; set; }
    /// <summary>
    /// Parent
    /// </summary>
    public int ParentId { get; set; }
}