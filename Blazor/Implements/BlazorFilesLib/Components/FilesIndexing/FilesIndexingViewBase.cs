////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorFilesLib.Components.FilesIndexing;

/// <summary>
/// FilesIndexingViewBase
/// </summary>
public class FilesIndexingViewBase : BlazorBusyComponentBaseAuthModel
{
    /// <inheritdoc/>
    [Inject]
    protected IIndexingServive FilesIndexingRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int FileId { get; set; }
}