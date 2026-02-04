////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// AttachesMessageWebChatModelDB
/// </summary>
public class AttachesMessageWebChatModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public MessageWebChatModelDB? MessageOwner { get; set; }
    /// <inheritdoc/>
    public int MessageOwnerId { get; set; }

    /// <inheritdoc/>
    public required int FileAttachId { get; set; }

    /// <inheritdoc/>
    public string? FileAttachName { get; set; }

    /// <summary>
    /// Размер файла
    /// </summary>
    public long FileLength { get; set; }

    /// <inheritdoc/>
    public string? FileTokenAccess { get; set; }

    /// <inheritdoc/>
    public string GetURI => $"/cloud-fs/read/{FileAttachId}/{FileAttachName}?{GlobalStaticConstantsRoutes.Routes.TOKEN_CONTROLLER_NAME}={FileTokenAccess}";
}