////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AttributeValueDaichiModelDB
/// </summary>
public class AttributeValueDaichiModelDB : EntryModel
{
    /// <inheritdoc/>
    public AttributeDaichiModelDB? Attribute { get; set; }

    /// <inheritdoc/>
    public int AttributeId { get; set; }


    /// <inheritdoc/>
    public GoodsDaichiModelDB? Goods { get; set; }

    /// <inheritdoc/>
    public int GoodsId { get; set; }


    /// <summary>
    /// Значение атрибута
    /// </summary>
    public string? ValueAttribute { get; set; }
}