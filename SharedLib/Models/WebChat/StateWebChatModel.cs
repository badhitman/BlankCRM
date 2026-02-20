////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// StateWebChatModel
/// </summary>
public class StateWebChatModel
{
    /// <inheritdoc/>
    public required int DialogId { get; set; }

    /// <inheritdoc/>
    public required bool StateDialog { get; set; }

    /// <inheritdoc/>
    public required string? UserIdentityId { get; set; }
}