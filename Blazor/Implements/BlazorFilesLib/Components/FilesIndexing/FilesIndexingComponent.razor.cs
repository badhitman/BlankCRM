////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorFilesLib.Components.FilesIndexing;

/// <summary>
/// FilesIndexingComponent
/// </summary>
public partial class FilesIndexingComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required StorageFileMiddleModel FileObject { get; set; }
}