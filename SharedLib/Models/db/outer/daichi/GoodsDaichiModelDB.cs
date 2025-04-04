////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// GoodsDaichiModelDB
/// </summary>
public class GoodsDaichiModelDB : GoodsDaichiBaseModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Значения атрибутов
    /// </summary>
    public List<AttributeValueDaichiModelDB>? AttributesValues { get; set; }

    /// <inheritdoc/>
    public required DateTime LastUpdatedAt { get; set; }
}