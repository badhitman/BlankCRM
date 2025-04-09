////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// TechProductBreezRuResponseModelDB
/// </summary>
public class TechProductBreezRuResponseModelDB : ProductBreezRuLiteModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public List<TechProductBreezRuModelDB>? Properties { get; set; }
}