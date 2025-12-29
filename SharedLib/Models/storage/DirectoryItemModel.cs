////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// DirectoryItemModel
/// </summary>
public class DirectoryItemModel
{
    /// <inheritdoc/>
    public required string FullPath { get; set; }

    /// <inheritdoc/>
    public bool IsDirectory { get; set; }

    /// <inheritdoc/>
    public long FileSizeBytes { get; set; }

    /// <inheritdoc/>
    public DateTime LastWriteTimeUtc { get; set; }
}