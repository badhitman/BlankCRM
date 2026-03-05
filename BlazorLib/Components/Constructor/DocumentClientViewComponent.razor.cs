////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Constructor;

/// <summary>
/// Client view
/// </summary>
/// <remarks>
/// Конечные пользователи (клиенты) будт заполнять  данными документ в контексте отдельной сессии
/// </remarks>
public partial class DocumentClientViewComponent : ComponentBase
{
    [Inject]
    ISnackbar SnackbarRepo { get; set; } = default!;


    /// <summary>
    /// Сессия для заполнения данными документа конечным пользователем (клиентом)
    /// </summary>
    [Parameter, EditorRequired]
    public required SessionOfDocumentDataModelDB SessionOfDocumentData { get; set; }

    /// <summary>
    /// Вызов команды для перезагрузки данных сессии
    /// </summary>
    [Parameter, EditorRequired]
    public required Action ReloadSessionHandler { get; set; }


    /// <summary>
    /// Информация
    /// </summary>
    protected MarkupString Information => (MarkupString)(!string.IsNullOrWhiteSpace(SessionOfDocumentData.Description) ? SessionOfDocumentData.Description : SessionOfDocumentData.Owner!.Description ?? "");
}