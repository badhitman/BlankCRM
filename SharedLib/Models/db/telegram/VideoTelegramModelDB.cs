////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// This object represents a video file.
/// </summary>
public class VideoTelegramModelDB : FileBaseTelegramModel
{
    /// <summary>
    /// Video width as defined by sender
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Video height as defined by sender
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Duration of the video in seconds as defined by sender
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Optional. Original filename as defined by sender
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Optional. Mime type of a file as defined by sender
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// ThumbnailVideo
    /// </summary>
    public VideoThumbnailTelegramModelDB? ThumbnailVideo { get; set; }

    /// <inheritdoc/>
    public static VideoTelegramStandardModel Build(VideoTelegramModelDB video)
    {
        VideoTelegramStandardModel res = new()
        {
            MessageId = video.MessageId,
            Message = video.Message,
            Id = video.Id,
            Height = video.Height,
            Duration = video.Duration,
            FileName = video.FileName,
            MimeType = video.MimeType,
            FileUniqueId = video.FileUniqueId,
            FileId = video.FileId,
            FileSize = video.FileSize,
            Width = video.Width,
        };
        res.ThumbnailVideo = video.ThumbnailVideo is null ? null : VideoThumbnailTelegramModelDB.Build(video.ThumbnailVideo, res);
        return res;
    }
}