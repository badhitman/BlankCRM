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

    /// <summary>
    /// Вложение/файл (опционально)
    /// </summary>
    public required int FileAttachId { get; set; }
}