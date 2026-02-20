////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public class TelegramJoinAccountConfirmModel
{
    /// <inheritdoc/>
    public required string Token { get; set; }

    /// <inheritdoc/>
    public required long TelegramId { get; set; }

    /// <inheritdoc/>
    public required uint TelegramJoinAccountTokenLifetimeMinutes { get; set; }

    /// <inheritdoc/>
    public required string ClearBaseUri { get; set; }
}