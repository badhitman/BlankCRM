////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// JoinUserChatViewModel
/// </summary>
public class JoinUserChatViewModel
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// User
    /// </summary>
    public virtual UserTelegramStandardModel? User { get; set; }
    /// <summary>
    /// User
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Chat
    /// </summary>
    public virtual ChatTelegramStandardModel? Chat { get; set; }
    /// <summary>
    /// Chat
    /// </summary>
    public int ChatId { get; set; }
}