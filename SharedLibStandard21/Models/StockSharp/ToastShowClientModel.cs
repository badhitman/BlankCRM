////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Toast notify for client`s
/// </summary>
public class ToastShowClientModel
{
    /// <inheritdoc/>
    public MessagesTypesEnum TypeMessage { get; set; }

    /// <inheritdoc/>
    public string? HeadTitle { get; set; }

    /// <inheritdoc/>
    public string? MessageText { get; set; }
}