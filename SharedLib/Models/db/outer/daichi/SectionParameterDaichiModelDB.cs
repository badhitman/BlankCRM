////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class SectionParameterDaichiModelDB : EntryModel
{
    /// <inheritdoc/>
    public ParameterEntryDaichiModelDB? Parent { get; set; }
    /// <inheritdoc/>
    public int ParentId { get; set; }
}