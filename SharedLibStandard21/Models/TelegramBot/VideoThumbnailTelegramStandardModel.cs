////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// VideoThumbnailTelegramStandardModel
/// </summary>
public class VideoThumbnailTelegramStandardModel : PhotoSizeTelegramStandardModel
{
    /// <summary>
    /// AudioOwner
    /// </summary>
    public virtual VideoTelegramStandardModel? VideoOwner { get; set; }
    /// <summary>
    /// AudioOwner
    /// </summary>
    public int VideoOwnerId { get; set; }
}