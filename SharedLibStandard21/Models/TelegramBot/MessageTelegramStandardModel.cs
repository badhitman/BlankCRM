using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// MessageTelegramStandardModel
/// </summary>
public class MessageTelegramStandardModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <inheritdoc/>
    public TelegramMessagesTypesEnum TypeMessage { get; set; }

    /// <summary>
    /// Message id (Telegram id)
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public int MessageTelegramId { get; set; }

    /// <summary>
    /// Optional. Unique identifier of a message thread to which the message belongs; for supergroups only
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int? MessageThreadId { get; set; }

    /// <summary>
    /// Optional. Sender, empty for messages sent to channels
    /// </summary>
    public virtual UserTelegramStandardModel? From { get; set; }

    /// <summary>
    /// Optional. Sender, empty for messages sent to channels
    /// </summary>
    public int? FromId { get; set; }

    /// <summary>
    /// Optional. Sender of the message, sent on behalf of a chat. The channel itself for channel messages.
    /// The supergroup itself for messages from anonymous group administrators. The linked channel for messages
    /// automatically forwarded to the discussion group
    /// </summary>
    public virtual ChatTelegramStandardModel? Chat { get; set; }

    /// <summary>
    /// Optional. Sender of the message, sent on behalf of a chat. The channel itself for channel messages.
    /// The supergroup itself for messages from anonymous group administrators. The linked channel for messages
    /// automatically forwarded to the discussion group
    /// </summary>
    public int ChatId { get; set; }

    /// <summary>
    /// Optional. Sender of the message, sent on behalf of a chat. The channel itself for channel messages.
    /// The supergroup itself for messages from anonymous group administrators. The linked channel for messages
    /// automatically forwarded to the discussion group
    /// </summary>
    public int? SenderChatId { get; set; }

    /// <summary>
    /// Optional. For forwarded messages, sender of the original message
    /// </summary>
    public long? ForwardFromId { get; set; }

    /// <summary>
    /// Optional. <see langword="true"/>, if the message is sent to a forum topic
    /// </summary>
    public bool? IsTopicMessage { get; set; }

    /// <summary>
    /// Optional. For messages forwarded from channels or from anonymous administrators, information about the
    /// original sender chat
    /// </summary>
    public long? ForwardFromChatId { get; set; }

    /// <summary>
    /// Optional. For messages forwarded from channels, identifier of the original message in the channel
    /// </summary>
    public int? ForwardFromMessageId { get; set; }

    /// <summary>
    /// Optional. For messages forwarded from channels, signature of the post author if present
    /// </summary>
    public string? ForwardSignature { get; set; }

    /// <summary>
    /// Optional. Sender's name for messages forwarded from users who disallow adding a link to their account in
    /// forwarded messages
    /// </summary>
    public string? ForwardSenderName { get; set; }

    /// <summary>
    /// Optional. For forwarded messages, date the original message was sent
    /// </summary>
    public DateTime? ForwardDate { get; set; }

    /// <summary>
    /// Optional. <see langword="true"/>, if the message is a channel post that was automatically forwarded to the connected
    /// discussion group
    /// </summary>
    public bool? IsAutomaticForward { get; set; }

    /// <summary>
    /// Ответ на сообщение (db:id).
    /// </summary>
    public int? ReplyToMessageId { get; set; }

    /// <summary>
    /// Optional. Bot through which the message was sent <see cref="UserTelegramStandardModel"/>
    /// </summary>
    public long? ViaBotId { get; set; }

    /// <summary>
    /// Optional. Date the message was last edited
    /// </summary>
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? EditDate { get; set; }

    /// <summary>
    /// Optional. The unique identifier of a media message group this message belongs to
    /// </summary>
    public string? MediaGroupId { get; set; }

    /// <summary>
    /// Optional. Signature of the post author for messages in channels, or the custom title of an anonymous
    /// group administrator
    /// </summary>
    public string? AuthorSignature { get; set; }

    /// <summary>
    /// Optional. For text messages, the actual text of the message, 0-4096 characters
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// CreatedAtUtc
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Optional. Caption for the animation, audio, document, photo, video or voice, 0-1024 characters
    /// </summary>
    public string? Caption { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    public virtual ContactTelegramStandardModel? Contact { get; set; }

    /// <summary>
    /// Voice
    /// </summary>
    public virtual VoiceTelegramStandardModel? Voice { get; set; }

    /// <summary>
    /// Audio
    /// </summary>
    public virtual AudioTelegramStandardModel? Audio { get; set; }

    /// <summary>
    /// Document
    /// </summary>
    public virtual DocumentTelegramStandardModel? Document { get; set; }

    /// <summary>
    /// Video
    /// </summary>
    public virtual VideoTelegramStandardModel? Video { get; set; }

    /// <summary>
    /// Optional. Message is a photo, available sizes of the photo
    /// </summary>
    public List<PhotoMessageTelegramStandardModel>? Photo { get; set; }
}