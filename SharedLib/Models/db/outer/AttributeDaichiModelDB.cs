////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AttributeDaichiModelDB
/// </summary>
public class AttributeDaichiModelDB : EntryModel
{
    /// <inheritdoc/>
    public List<AttributeValueDaichiModelDB>? AttributesValues { get; set; }

    /// <inheritdoc/>
    public GroupAttributeDaichiModelDB? Group { get; set; }

    /// <inheritdoc/>
    public int GroupId { get; set; }
}
