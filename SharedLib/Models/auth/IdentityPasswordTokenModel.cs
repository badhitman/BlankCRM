////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IdentityPasswordTokenModel
/// </summary>
public class IdentityPasswordTokenModel: IdentityPasswordModel
{
    /// <summary>
    /// Token
    /// </summary>
    public required string Token { get; set; }
}