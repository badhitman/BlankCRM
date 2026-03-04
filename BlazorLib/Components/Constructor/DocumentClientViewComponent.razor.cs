////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// DocumentScheme client view
/// </summary>
public partial class DocumentClientViewComponent : ComponentBase
{
    [Inject]
    ISnackbar SnackbarRepo { get; set; } = default!;


    /// <summary>
    /// Session questionnaire
    /// </summary>
    [Parameter, EditorRequired]
    public required SessionOfDocumentDataModelDB SessionOfDocumentData { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action ReloadHandler { get; set; }


    /// <summary>
    /// Информация
    /// </summary>
    protected MarkupString Information => (MarkupString)(!string.IsNullOrWhiteSpace(SessionOfDocumentData.Description) ? SessionOfDocumentData.Description : SessionOfDocumentData.Owner!.Description ?? "");
}