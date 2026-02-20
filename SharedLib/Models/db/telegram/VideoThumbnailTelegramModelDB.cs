////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// VideoThumbnailTelegramModelDB
/// </summary>
public class VideoThumbnailTelegramModelDB : PhotoSizeTelegramModel
{
    /// <summary>
    /// AudioOwner
    /// </summary>
    public VideoTelegramModelDB? VideoOwner { get; set; }
    /// <summary>
    /// AudioOwner
    /// </summary>
    public int VideoOwnerId { get; set; }

    /// <inheritdoc/>
    public static VideoThumbnailTelegramStandardModel Build(VideoThumbnailTelegramModelDB sender, VideoTelegramStandardModel owner)
    {
        return new()
        {
            Id = sender.Id,
            Width = sender.Width,
            Height = sender.Height,
            FileId = sender.FileId,
            Message = sender.Message,
            FileSize = sender.FileSize,
            FileUniqueId = sender.FileUniqueId,
            MessageId = sender.MessageId,
            VideoOwnerId = owner.Id,
            VideoOwner = owner,
        };
    }
}