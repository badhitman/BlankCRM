////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DocumentThumbnailTelegramModelDB
/// </summary>
public class DocumentThumbnailTelegramModelDB : PhotoSizeTelegramModel
{
    /// <summary>
    /// Document
    /// </summary>
    public DocumentTelegramModelDB? DocumentOwner { get; set; }
    /// <summary>
    /// Document
    /// </summary>
    public int DocumentOwnerId { get; set; }

    /// <inheritdoc/>
    public static DocumentThumbnailTelegramStandardModel? Build(DocumentThumbnailTelegramModelDB sender, DocumentTelegramStandardModel owner)
    {
        return new()
        {
            Id = sender.Id,
            Width = sender.Width,
            Height = sender.Height,
            FileId = sender.FileId,
            Message = sender.Message,
            FileSize = sender.FileSize,
            MessageId = sender.MessageId,
            FileUniqueId = sender.FileUniqueId,
            DocumentOwnerId = sender.DocumentOwnerId,
            DocumentOwner = owner,
        };
    }
}