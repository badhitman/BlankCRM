////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FirebaseSDKConfigModel
/// </summary>
public class FirebaseSDKConfigModel
{
    /// <inheritdoc/>
    public string? ApiKey { get; set; }

    /// <inheritdoc/>
    public string? AuthDomain { get; set; }

    /// <inheritdoc/>
    public string? DatabaseURL { get; set; }

    /// <inheritdoc/>
    public string? ProjectId { get; set; }

    /// <inheritdoc/>
    public string? StorageBucket { get; set; }

    /// <inheritdoc/>
    public string? MessagingSenderId { get; set; }

    /// <inheritdoc/>
    public string? AppId { get; set; }

    /// <inheritdoc/>
    public string? MeasurementId { get; set; }

    /// <inheritdoc/>
    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(ApiKey) &&
        !string.IsNullOrWhiteSpace(AuthDomain) &&
        !string.IsNullOrWhiteSpace(DatabaseURL) &&
        !string.IsNullOrWhiteSpace(ProjectId) &&
        !string.IsNullOrWhiteSpace(StorageBucket) &&
        !string.IsNullOrWhiteSpace(MessagingSenderId) &&
        !string.IsNullOrWhiteSpace(AppId) &&
        !string.IsNullOrWhiteSpace(MeasurementId);

    /// <summary>
    /// FirebaseSDKConfig
    /// </summary>
    public static readonly string Configuration = "FirebaseSDKConfig";
}