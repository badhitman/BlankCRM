////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ShowToastEventModel
/// </summary>
public class ShowToastEventModel
{
    /// <inheritdoc/>
    public int DialogId { get; set; }

    /// <inheritdoc/>
    public required string Heading { get; set; }

    /// <inheritdoc/>
    public required string Text { get; set; }

    /// <inheritdoc/>
    public string? Icon { get; set; }

    /// <inheritdoc/>
    public bool Loader { get; set; }

    /// <inheritdoc/>
    public string LoaderBg { get; set; } = "#9EC600";
}