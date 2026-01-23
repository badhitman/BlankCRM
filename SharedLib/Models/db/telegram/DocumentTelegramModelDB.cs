////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// This object represents a general file (as opposed to <see cref="PhotoSizeTelegramModel">photos</see>, <see cref="VoiceTelegramModelDB">voice messages</see> and <see cref="AudioTelegramModelDB">audio files</see>).
/// </summary>
public class DocumentTelegramModelDB : FileBaseTelegramModel
{
    /// <summary>
    /// Optional. Original filename as defined by sender
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Optional. MIME type of the file as defined by sender
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// ThumbnailDocument
    /// </summary>
    public DocumentThumbnailTelegramModelDB? ThumbnailDocument { get; set; }

    /// <inheritdoc/>
    public static DocumentTelegramStandardModel Build(DocumentTelegramModelDB document)
    {
        DocumentTelegramStandardModel res = new()
        {
            Message = document.Message,
            Id = document.Id,
            FileId = document.FileId,
            FileName = document.FileName,
            MimeType = document.MimeType,
            FileSize = document.FileSize,
            FileUniqueId = document.FileUniqueId,
            MessageId = document.MessageId,
        };
        res.ThumbnailDocument = document.ThumbnailDocument is null ? null : DocumentThumbnailTelegramModelDB.Build(document.ThumbnailDocument, res);
        return res;
    }
}