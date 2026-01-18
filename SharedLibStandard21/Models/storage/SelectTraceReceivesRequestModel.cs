////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// SelectTraceReceivesRequestModel
/// </summary>
public class SelectTraceReceivesRequestModel
{
    /// <inheritdoc/>
    public string[]? ReceiversNames { get; set; }

    /// <summary>
    /// IdentityUserId
    /// </summary>
    [Required]
    public string[]? IdentityUsersIds { get; set; }
}