////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// This object represents a general file (as opposed to <see cref="PhotoSizeTelegramStandardModel">photos</see>, <see cref="VoiceTelegramStandardModel">voice messages</see> and <see cref="AudioTelegramStandardModel">audio files</see>).
/// </summary>
public class DocumentTelegramStandardModel : FileBaseTelegramStandardModel
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
    public DocumentThumbnailTelegramStandardModel? ThumbnailDocument { get; set; }
}