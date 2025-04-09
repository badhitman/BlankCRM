////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechProductBreezRuModelDB
/// </summary>
public class TechProductBreezRuModelDB : TechProductRealBreezRuModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public TechProductBreezRuResponseModelDB? Parent { get; set; }
    /// <inheritdoc/>
    public int ParentId { get; set; }
}