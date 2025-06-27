////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ToastModel
/// </summary>
public class ToastShowClientModel
{
    /// <inheritdoc/>
    public MessagesTypesEnum TypeMessage { get; set; }

    /// <inheritdoc/>
    public string HeadTitle { get; set; }

    /// <inheritdoc/>
    public string MessageText { get; set; }
}