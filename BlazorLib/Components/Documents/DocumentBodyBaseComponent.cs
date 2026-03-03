////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;

namespace BlazorLib;

/// <summary>
/// DocumentBodyBaseComponent
/// </summary>
public abstract class DocumentBodyBaseComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    public IJournalUniversalService JournalRepo { get; set; } = default!;


    /// <summary>
    /// ID (html dom)
    /// </summary>
    [Parameter, EditorRequired]
    public required string ID { get; set; }


    /// <inheritdoc/>
    public DocumentFitModel? DocumentMetadata { get; set; }

    /// <summary>
    /// PK строки БД.
    /// </summary>
    /// <remarks>
    /// Если null, то demo решим. Если HasValue и меньше 1, тогда создание нового объекта
    /// </remarks>
    [Parameter]
    public int? DocumentKey { get; set; }


    /// <summary>
    /// IsEdited
    /// </summary>
    public abstract bool IsEdited { get; }
}