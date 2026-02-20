////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// This object represents a phone contact.
/// </summary>
public class ContactTelegramStandardModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    public virtual int Id { get; set; }

    /// <summary>
    /// Contact's phone number
    /// </summary>
    public virtual string? PhoneNumber { get; set; }

    /// <summary>
    /// Contact's first name
    /// </summary>
    public virtual string? FirstName { get; set; }

    /// <summary>
    /// Optional. Contact's last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Optional. Contact's user identifier in Telegram
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Optional. Additional data about the contact in the form of a vCard
    /// </summary>
    public string? Vcard { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public int MessageId { get; set; }
    /// <summary>
    /// Message
    /// </summary>
    public virtual MessageTelegramStandardModel? Message { get; set; }
}