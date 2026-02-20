////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AudioThumbnailTelegramModelDB
/// </summary>
public class AudioThumbnailTelegramStandardModel : PhotoSizeTelegramStandardModel
{
    /// <summary>
    /// AudioOwner
    /// </summary>
    public virtual AudioTelegramStandardModel? AudioOwner { get; set; }
    /// <summary>
    /// AudioOwner
    /// </summary>
    public int AudioOwnerId { get; set; }
}