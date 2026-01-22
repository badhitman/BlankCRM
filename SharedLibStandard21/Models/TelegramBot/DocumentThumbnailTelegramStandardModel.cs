////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DocumentThumbnailTelegramStandardModel
/// </summary>
public class DocumentThumbnailTelegramStandardModel : PhotoSizeTelegramStandardModel
{
    /// <summary>
    /// Document
    /// </summary>
    public virtual DocumentTelegramStandardModel? DocumentOwner { get; set; }
    /// <summary>
    /// Document
    /// </summary>
    public int DocumentOwnerId { get; set; }
}