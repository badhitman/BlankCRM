////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AudioThumbnailTelegramModelDB
/// </summary>
public class AudioThumbnailTelegramModelDB : PhotoSizeTelegramModel
{
    /// <summary>
    /// AudioOwner
    /// </summary>
    public AudioTelegramModelDB? AudioOwner { get; set; }
    /// <summary>
    /// AudioOwner
    /// </summary>
    public int AudioOwnerId { get; set; }

    /// <inheritdoc/>
    public static AudioThumbnailTelegramStandardModel Build(AudioThumbnailTelegramModelDB sender, AudioTelegramStandardModel ownerAudio)
    {
        return new()
        {
            MessageId = sender.MessageId,
            AudioOwnerId = sender.AudioOwnerId,
            FileId = sender.FileId,
            FileSize = sender.FileSize,
            FileUniqueId = sender.FileUniqueId,
            Height = sender.Height,
            Id = sender.Id,
            Message = sender.Message,
            Width = sender.Width,
            AudioOwner = ownerAudio,
        };
    }
}