////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// SelectRequestAuthBaseModel
/// </summary>
public class SelectRequestAuthBaseModel : SelectRequestBaseModel
{
    /// <summary>
    /// IdentityUserId
    /// </summary>
    [Required]
    public string[]? IdentityUsersIds { get; set; }
}