////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PhotoMessageTelegramModelDB
/// </summary>
public class PhotoMessageTelegramModelDB : PhotoSizeTelegramModel
{
    /// <inheritdoc/>
    public static PhotoMessageTelegramStandardModel Build(PhotoMessageTelegramModelDB sender)
    {
        return new()
        {
            Id = sender.Id,
            Width = sender.Width,
            Height = sender.Height,
            FileId = sender.FileId,
            FileSize = sender.FileSize,
            FileUniqueId = sender.FileUniqueId,
            Message = sender.Message,
            MessageId = sender.MessageId,
        };
    }
}