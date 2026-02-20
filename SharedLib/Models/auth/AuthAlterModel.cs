////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AuthAlterModel
/// </summary>
public class AuthAlterModel : IdentityPasswordVersionModel
{
    /// <summary>
    /// Version
    /// </summary>
    public required string PartnerId { get; set; }
}