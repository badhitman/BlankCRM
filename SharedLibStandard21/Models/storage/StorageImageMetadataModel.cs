////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// StorageImageMetadataModel
/// </summary>
public class StorageImageMetadataModel : StorageMetadataModel
{
    /// <summary>
    /// FileName
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// ContentType
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Referer
    /// </summary>
    public string Referrer { get; set; }

    /// <summary>
    /// Payload
    /// </summary>
    public byte[] Payload { get; set; }

    /// <summary>
    /// AuthorUserIdentity
    /// </summary>
    public string AuthorUserIdentity { get; set; }
}