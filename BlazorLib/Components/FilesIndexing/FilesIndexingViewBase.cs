////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.FilesIndexing;

/// <summary>
/// FilesIndexingViewBase
/// </summary>
public class FilesIndexingViewBase : BlazorBusyComponentBaseAuthModel
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int FileId { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

    }
}