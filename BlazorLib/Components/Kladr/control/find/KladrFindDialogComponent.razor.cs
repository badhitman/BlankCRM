////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using MudBlazor;

namespace BlazorLib.Components.Kladr.control.find;

/// <summary>
/// KladrFindDialogComponent
/// </summary>
public partial class KladrFindDialogComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = default!;

    /// <inheritdoc/>
    [Parameter]
    public string? FindText { get; set; }


    private void Cancel() => MudDialog.Cancel();
}