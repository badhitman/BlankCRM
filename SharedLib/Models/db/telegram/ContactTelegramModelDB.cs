////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// This object represents a phone contact.
/// </summary>
public class ContactTelegramModelDB : ContactTelegramStandardModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public override int Id { get; set; }

    /// <summary>
    /// Contact's phone number
    /// </summary>
    public new required string PhoneNumber { get; set; }

    /// <summary>
    /// Contact's first name
    /// </summary>
    public new required string FirstName { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public new MessageTelegramModelDB? Message { get; set; }
}