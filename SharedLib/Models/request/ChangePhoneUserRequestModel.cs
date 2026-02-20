////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InitChangePhoneUserRequestModel
/// </summary>
public class ChangePhoneUserRequestModel
{
    /// <summary>
    /// UserId
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// PhoneNum
    /// </summary>
    public string? PhoneNum { get; set; }
}