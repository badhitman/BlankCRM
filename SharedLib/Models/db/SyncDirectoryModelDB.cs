////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Папка синхронизации
/// </summary>
public class SyncDirectoryModelDB : ApiRestBaseModel
{
    /// <summary>
    /// LocalDirectory
    /// </summary>
    public string? LocalDirectory { get; set; }

    /// <summary>
    /// RemoteDirectory
    /// </summary>
    public string? RemoteDirectory { get; set; }

    /// <inheritdoc/>
    public void Update(SyncDirectoryModelDB other)
    {
        LocalDirectory = other.LocalDirectory;
        RemoteDirectory = other.RemoteDirectory;
        Name = other.Name;
        Id = other.Id;
    }

    /// <inheritdoc/>
    public static SyncDirectoryModelDB BuildEmpty(int tokenId)
    {
        return new()
        {
            LocalDirectory = string.Empty,
            RemoteDirectory = string.Empty,
            Name = string.Empty,
            ParentId = tokenId
        };
    }
}