////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// AboutUserAgentModel
/// </summary>
public class AboutUserAgentModel
{
    /// <inheritdoc/>
    public string? UserAgent { get; set; }

    /// <inheritdoc/>
    public string? Language { get; set; }

    /// <inheritdoc/>
    public bool CookieEnabled { get; set; }
}