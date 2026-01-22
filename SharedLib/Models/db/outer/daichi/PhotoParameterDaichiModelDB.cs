////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class PhotoParameterDaichiModelDB : EntryStandardModel
{
    /// <inheritdoc/>
    public ParameterEntryDaichiModelDB? Parent { get; set; }
    /// <inheritdoc/>
    public int ParentId { get; set; }
}