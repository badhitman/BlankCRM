////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// PropertyTechProductBreezRuModelDB
/// </summary>
public class PropertyTechProductBreezRuModelDB : PropTechProductBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public int Key { get; set; }

    /// <inheritdoc/>
    public TechProductBreezRuModelDB? Parent { get; set; }
    /// <inheritdoc/>
    public int ParentId { get; set; }
}