////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// InitChangePhoneUserRequestModel
/// </summary>
public class InitChangePhoneUserRequestModel
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