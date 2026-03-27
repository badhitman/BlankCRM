////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SendFirebaseMessageRequestModel
/// </summary>
public class SendFirebaseMessageRequestModel
{
    /// <inheritdoc/>
    public required List<string> TokensFCM { get; set; }

    /// <inheritdoc/>
    public required string Title { get; set; }

    /// <inheritdoc/>
    public required string TextBody { get; set; }

    /// <inheritdoc/>
    public string? Name { get; set; }

    /// <inheritdoc/>
    public bool ExpandViewMode { get; set; }

    /// <inheritdoc/>
    public string? ImageUrl { get; set; }

    /// <inheritdoc/>
    public Dictionary<string, string>? Data { get; set; }

    /// <inheritdoc/>
    public bool IsValid =>
        TokensFCM.Count != 0 &&
        TokensFCM.All(x => !string.IsNullOrWhiteSpace(x)) &&
        !string.IsNullOrWhiteSpace(Title) &&
        !string.IsNullOrWhiteSpace(TextBody);
}