////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// MessageTelegramModelDB
/// </summary>
[Index(nameof(MessageTelegramId), nameof(ChatId), nameof(FromId))]
public class MessageTelegramModelDB : MessageTelegramViewModel
{
    /// <summary>
    /// Optional. Sender, empty for messages sent to channels
    /// </summary>
    public new UserTelegramModelDB? From { get; set; }

    /// <summary>
    /// Optional. Sender of the message, sent on behalf of a chat. The channel itself for channel messages.
    /// The supergroup itself for messages from anonymous group administrators. The linked channel for messages
    /// automatically forwarded to the discussion group
    /// </summary>
    public new ChatTelegramModelDB? Chat { get; set; }

    /// <summary>
    /// Ответ на сообщение
    /// </summary>
    [NotMapped]
    public MessageTelegramModelDB? ReplyToMessage { get; set; }

    /// <summary>
    /// Optional. Message is a photo, available sizes of the photo
    /// </summary>
    public List<PhotoMessageTelegramModelDB>? Photo { get; set; }

    /// <summary>
    /// Audio
    /// </summary>
    public int? AudioId { get; set; }
    /// <summary>
    /// Audio
    /// </summary>
    public AudioTelegramModelDB? Audio { get; set; }

    /// <summary>
    /// Video
    /// </summary>
    public VideoTelegramModelDB? Video { get; set; }
    /// <summary>
    /// Video
    /// </summary>
    public int? VideoId { get; set; }

    /// <summary>
    /// Document
    /// </summary>
    public DocumentTelegramModelDB? Document { get; set; }
    /// <summary>
    /// Document
    /// </summary>
    public int? DocumentId { get; set; }


    /// <summary>
    /// Voice
    /// </summary>
    public VoiceTelegramModelDB? Voice { get; set; }
    /// <summary>
    /// Voice
    /// </summary>
    public int? VoiceId { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    public ContactTelegramModelDB? Contact { get; set; }
    /// <summary>
    /// Contact
    /// </summary>
    public int? ContactId { get; set; }

    /// <summary>
    /// SenderChat
    /// </summary>
    [NotMapped]
    public ChatTelegramModelDB? SenderChat { get; set; }

    /// <summary>
    /// ForwardFrom
    /// </summary>
    [NotMapped]
    public UserTelegramModelDB? ForwardFrom { get; set; }
}