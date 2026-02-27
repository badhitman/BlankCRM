////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GetCurrentMainProjectRequestModel
/// </summary>
public class GetCurrentMainProjectRequestModel
{
    /// <inheritdoc/>
    public required string UserIdentityId { get; set; }

    /// <inheritdoc/>
    public string? ContextName { get; set; }
}